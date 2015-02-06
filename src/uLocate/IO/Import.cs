namespace uLocate.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Configuration;
    using System.Text;
    using System.Threading.Tasks;
    using FileHelpers;
    using FileHelpers.Dynamic;

    using StackExchange.Profiling;

    using uLocate.Data;
    using uLocate.Helpers;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Core.Logging;

    using Constants = uLocate.Constants;

    public static class Import
    {
        #region CSV Importing
        public static StatusMessage LocationsCSV(string FilePath, Guid? LocationTypeKey = null)
        {
            Guid LocTypeKey;
            if (LocationTypeKey == null)
            {
                LocTypeKey = Constants.DefaultLocationTypeKey;
            }
            else
            {
                LocTypeKey = (Guid)LocationTypeKey;
            }


            StatusMessage Msg = new StatusMessage();
            Msg.ObjectName = FilePath;

            // Using http://filehelpers.sourceforge.net/ to process CSV files 
            // Dynamic class definition
            string dynamicClassDef = DynamicLocationFlatClass(LocTypeKey);

            Type t = ClassBuilder.ClassFromString(dynamicClassDef);

            FileHelperEngine fhEngine = new FileHelperEngine(t);

            //DataTable = engine.ReadFileAsDT("test.txt");

            //FileHelperEngine fhEngine = new FileHelperEngine(typeof(LocationFlat));

            string FullPath = Mindfly.Files.GetMappedPath(FilePath);
            var MsgDetails = new StringBuilder();

            try
            {
                //LocationFlat[] csvLocationFlats = fhEngine.ReadFile(FullPath) as LocationFlat[];
                var csvLocationFlats = fhEngine.ReadFile(FullPath);

                int RowsTotal = csvLocationFlats.Count();
                int RowsSuccess = 0;
                int RowsFailure = 0;
                int GeocodeCount = 0;
                bool NeedsMaintenance = false;

                foreach (dynamic row in csvLocationFlats)
                {
                    if (NeedsGeocoding(row))
                    {
                        GeocodeCount++;
                    }

                    var RowStatusMsg = ImportItem(row, LocTypeKey, GeocodeCount);
                    if (RowStatusMsg.Success)
                    {
                        RowsSuccess++;
                        if (StringExtensions.IsNullOrWhiteSpace(RowStatusMsg.Code))
                        {
                            MsgDetails.AppendLine(string.Format("'{0}' was imported successfully.", row.LocationName));
                        }
                        else
                        {
                            MsgDetails.AppendLine(string.Format("'{0}' was imported with some issues: {1}", row.LocationName, RowStatusMsg.Message));

                            if (RowStatusMsg.Code.Contains("GeocodingProblem") || RowStatusMsg.Code.Contains("UnableToUpdateDBGeography"))
                            {
                                NeedsMaintenance = true;
                            }
                        }

                    }
                    else
                    {
                        MsgDetails.AppendLine(string.Format("Error importing '{0}' : {1}", row.LocationName, RowStatusMsg.Message));
                        RowsFailure++;
                    }

                    if (RowStatusMsg.RelatedException != null)
                    {
                        LogHelper.Error(typeof(Import), string.Format("Error on '{0}'", row.LocationName), RowStatusMsg.RelatedException);
                    }
                }


                //Compile final status message
                Msg.Message += string.Format(
                    "Out of {0} total locations, {1} were imported successfully and {2} failed.",
                    RowsTotal,
                    RowsSuccess,
                    RowsFailure);

                if (NeedsMaintenance)
                {
                    Msg.Message += " Some rows encountered issues which might be fixed by running some maintenance tasks.";
                }

                Msg.MessageDetails = MsgDetails.ToString();
                Msg.Success = true;
                LogHelper.Info(typeof(Import), string.Format("Final Status importing file '{0}': {1} : {2}", FilePath, Msg.Message, Msg.MessageDetails));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("found after the last field"))
                {
                    Msg.Message = "The imported file data doesn't match the correct format. Please check that your columns are defined correctly.";
                    Msg.Success = false;
                    Msg.Code = ex.GetType().ToString();
                    Msg.RelatedException = ex;
                    Msg.ObjectName = FilePath;
                    LogHelper.Error(typeof(Import), string.Format("Error importing file '{0}'", FilePath), ex);
                }
                else
                {
                    Msg.Message = string.Format("An error occurred : {0}", ex.Message);
                    Msg.Success = false;
                    Msg.Code = ex.GetType().ToString();
                    Msg.RelatedException = ex;
                    LogHelper.Error(typeof(Import), string.Format("Error importing file '{0}'", FilePath), ex);
                }
            }

            return Msg;
        }

        private static StatusMessage ImportItem(dynamic locFlat, Guid LocationTypeKey, int GeocodeCount)
        {
            var ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = locFlat.LocationName;
            ReturnMsg.Success = true;
            var Msg = new StringBuilder();

            try
            {
                //Create new Location
                var newLoc = new Location(locFlat.LocationName, LocationTypeKey);

                //var newKey = newLoc.Key;
                Repositories.LocationRepo.Insert(newLoc);
                //newLoc = Repositories.LocationRepo.GetByKey(newKey);

                //Set default properties
                newLoc.AddPropertyData("Address1", locFlat.Address1);
                newLoc.AddPropertyData("Address2", locFlat.Address2);
                newLoc.AddPropertyData("Locality", locFlat.Locality);
                newLoc.AddPropertyData("Region", locFlat.Region);
                newLoc.AddPropertyData("PostalCode", locFlat.PostalCode);
                newLoc.AddPropertyData("CountryCode", locFlat.CountryCode);
                newLoc.AddPropertyData("PhoneNumber", locFlat.PhoneNumber);
                newLoc.AddPropertyData("Email", locFlat.Email);

                if (LocationTypeKey != Constants.DefaultLocationTypeKey)
                {
                    var CustomLtProps = Repositories.LocationTypePropertyRepo.GetByLocationType(LocationTypeKey);

                    //We need to copy the properties into their own list 
                    //to avoid the "Collection was modified; enumeration operation may not execute." error
                    var propsCollection = new List<LocationTypeProperty>(); 

                    foreach (var customProp in CustomLtProps)
                    {
                        propsCollection.Add(customProp);
                    }

                    foreach (var prop in propsCollection)
                    {
                        var importValue = locFlat.GetProperty(prop.Alias);

                        if (prop.DataType.PropertyEditorAlias == "Umbraco.DropDown")
                        {
                            //convert to prevalue node Id
                            var idValue = DataValuesHelper.GetPreValueId(prop.DataTypeId, importValue);
                            newLoc.AddPropertyData(prop.Alias, idValue);
                        } 
                            ////TODO: Add conversion checks for Boolean values as well (Yes/No, True/False, data/Null) to convert to 1/0
                        else
                        {
                            //Just import the data as-is
                            newLoc.AddPropertyData(prop.Alias, importValue);
                        }
                    }
                }

                //Attempt geocoding
                if (NeedsGeocoding(locFlat))
                {
                    if (GeocodeCount >= 400)
                    {
                        ReturnMsg.Success = true;
                        ReturnMsg.Code = "GeocodingProblem";
                        Msg.AppendLine("This address exceeded the limits for geo-coding in a batch.");
                    }
                    else
                    {
                        try
                        {
                            var locAddress = new Address()
                                                 {
                                                     Address1 = locFlat.Address1,
                                                     Address2 = locFlat.Address2,
                                                     Locality = locFlat.Locality,
                                                     Region = locFlat.Region,
                                                     PostalCode = locFlat.PostalCode,
                                                     CountryCode = locFlat.CountryCode
                                                 };

                            //TODO: make dynamic based on provider limit
                            var newCoordinate = DoGeocoding.GetCoordinateForAddress(locAddress);

                            newLoc.Latitude = newCoordinate.Latitude;
                            newLoc.Longitude = newCoordinate.Longitude;
                        }
                        catch (Exception e1)
                        {
                            ReturnMsg.Success = true;
                            ReturnMsg.RelatedException = e1;
                            ReturnMsg.Code = "GeocodingProblem";
                            Msg.AppendLine("There was a problem geo-coding the address.");
                            LogHelper.Error(typeof(Import), string.Format("Geo-coding error while importing '{0}'", ReturnMsg.ObjectName), e1);
                        }
                    }
                }
                else
                {
                    newLoc.Latitude = locFlat.Latitude;
                    newLoc.Longitude = locFlat.Longitude;
                }


                // SAVE properties of new location to db
                try
                {
                    //Repositories.LocationRepo.Insert(newLoc);
                    Repositories.LocationRepo.Update(newLoc);
                }
                catch (Exception e2)
                {
                    ReturnMsg.Success = false;
                    ReturnMsg.RelatedException = e2;
                    ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",InsertError" : "InsertError";
                    Msg.AppendLine("There was a problem saving the new location data.");
                }

                // UPDATE Geography field using Lat/Long
                try
                {
                    if (newLoc.Latitude != 0 && newLoc.Longitude != 0)
                    {
                        Repositories.LocationRepo.UpdateDbGeography(newLoc);
                    }
                    else
                    {
                        newLoc.DbGeogNeedsUpdated = true;
                        Repositories.LocationRepo.Update(newLoc);
                    }
                }
                catch (Exception e3)
                {
                    ReturnMsg.Success = true;
                    ReturnMsg.RelatedException = e3;
                    ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",UnableToUpdateDBGeography" : "UnableToUpdateDBGeography";
                    Msg.AppendLine("Unable to update the coordinates in the database.");
                }
            }
            catch (Exception ex)
            {
                ReturnMsg.Success = false;
                ReturnMsg.RelatedException = ex;
                ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",UnknownException" : "UnknownException";
                Msg.AppendLine(ex.Message);
                LogHelper.Error(typeof(Import), string.Format("ImportItem: Error importing location '{0}'", locFlat.LocationName), ex);
            }

            ReturnMsg.Message = Msg.ToString();
            return ReturnMsg;
        }

        #endregion

        #region Custom Class Definition

        private static string DynamicLocationFlatClass(Guid LocationTypeKey)
        {
            var classString = new StringBuilder();

            //Default props
            classString.Append(
                @"	[DelimitedRecord("","")]
                    [IgnoreEmptyLines()]
                    [IgnoreFirst(1)] 
                    public class LocationFlat
                    {
                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string LocationName;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Address1;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Address2;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Locality;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Region;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string PostalCode;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string CountryCode;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string PhoneNumber;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Email;

                        [FieldNullValue(typeof(double), ""0"")]
                        public double Longitude;

                        [FieldNullValue(typeof(double), ""0"")]
                        public double Latitude;

			");

            //Custom props
            if (LocationTypeKey != Constants.DefaultLocationTypeKey)
            {
                var CustomLtProps = Repositories.LocationTypePropertyRepo.GetByLocationType(LocationTypeKey);

                //Property Definitions
                foreach (var customProp in CustomLtProps.OrderBy(p => p.SortOrder))
                {
                    string fhProperty = FormatCustomProperty(customProp);
                    classString.Append(fhProperty);
                }

                //Custom 'GetProperty()' Methods
                //string fhStringMethod = GenerateCustomStringMethods(
                //    CustomLtProps.Where(p => p.DataType.DatabaseType == CmsDataType.DbType.Ntext || p.DataType.DatabaseType == CmsDataType.DbType.Nvarchar));
                string fhStringMethod = GenerateCustomStringMethods(CustomLtProps);
      
                classString.Append(fhStringMethod);

                //string fhIntegerMethod = GenerateCustomIntegerMethods(
                //    CustomLtProps.Where(p => p.DataType.DatabaseType == CmsDataType.DbType.Integer));
                //classString.Append(fhIntegerMethod);

                //string fhDateMethod = GenerateCustomDateMethods(
                //    CustomLtProps.Where(p => p.DataType.DatabaseType == CmsDataType.DbType.Date));
                //classString.Append(fhDateMethod);
            }

            //Close the class
            classString.Append("}");

            return classString.ToString();
        }

        private static string GenerateCustomStringMethods(IEnumerable<LocationTypeProperty> CustomProps)
        {
            var classMethod = new StringBuilder();


            classMethod.Append(
            @" public string GetProperty(string Alias)
                {
                    switch (Alias)
                    {
            ");

            foreach (var prop in CustomProps)
            {
                switch (prop.DataType.DatabaseType)
                {
                    case CmsDataType.DbType.Ntext:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0};
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Nvarchar:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0};
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Integer:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0}.ToString();
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Date:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0}.ToString();
                                break;
                                ", prop.Alias));
                        break;
                }
            }

            classMethod.Append(
            @"
                        default:
                            return """";
                    }
                }
            ");

            return classMethod.ToString();
        }

//        private static string GenerateCustomDateMethods(IEnumerable<LocationTypeProperty> CustomProps)
//        {
//            var classMethod = new StringBuilder();

//            classMethod.Append(
//            @" public DateTime GetProperty(string Alias)
//                {
//                    switch (Alias)
//                    {
//            ");

//            foreach (var prop in CustomProps)
//            {
//                classMethod.Append(string.Format(
//                @"
//                            case ""{0}"":
//                                return this.{0};
//                                break;
//                ", prop.Alias));
//            }

//            classMethod.Append(
//            @"
//                        default:
//                            return DateTime.MinValue;
//                    }
//                }
//            ");

//            return classMethod.ToString();
            
//        }

//        private static string GenerateCustomIntegerMethods(IEnumerable<LocationTypeProperty> CustomProps)
//        {
//            var classMethod = new StringBuilder();

//            classMethod.Append(
//            @" public Integer GetProperty(string Alias)
//                {
//                    switch (Alias)
//                    {
//            ");

//            foreach (var prop in CustomProps)
//            {
//                classMethod.Append(string.Format(
//                @"
//                            case ""{0}"":
//                                return this.{0};
//                                break;
//                ", prop.Alias));
//            }

//            classMethod.Append(
//            @"
//                        default:
//                            return 0;
//                    }
//                }
//            ");

//            return classMethod.ToString();
//        }  

        private static string FormatCustomProperty(LocationTypeProperty CustomProperty)
        {
            var classProp = new StringBuilder();

            switch (CustomProperty.DataType.DatabaseType)
            {
                case CmsDataType.DbType.Date:
                    classProp.Append(
                        string.Format(
                        @"  [FieldConverter(ConverterKind.Date, ""MMddyyyy"")]
            	            public DateTime {0};
                        ",
                            CustomProperty.Alias));
                    break;
                case CmsDataType.DbType.Integer:
                    classProp.Append(
                        string.Format(
                        @"  [FieldNullValue(typeof(int), ""0"")]
                            public int {0};
                        ",
                            CustomProperty.Alias));
                    break;
                default:
                    classProp.Append(
                        string.Format(
                        @"  [FieldNullValue(typeof(string), """")]
                            [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                            public string {0};
                                            ",
                            CustomProperty.Alias));
                    break;
            }

            return classProp.ToString();
        }

        #endregion

        #region General Functions

        /// <summary>
        /// Test whether this data needs to be geo-coded.
        /// </summary>
        /// <param name="locFlat">
        /// The location flat record
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool NeedsGeocoding(dynamic locFlat)
        {
            bool Result = true;
            if (locFlat.Latitude != 0 && locFlat.Longitude != 0)
            {
                Result = false;
            }

            return Result;
        }

        #endregion
    }
}
