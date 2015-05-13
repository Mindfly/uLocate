namespace uLocate.IO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
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
    using uLocate.Services;

    using umbraco.cms.businesslogic;

    using Umbraco.Core;
    using Umbraco.Core.Logging;

    using Constants = uLocate.Constants;

    public static class Import
    {
        const int MAX_GEOCODE = 400;

        #region CSV Importing
        public static StatusMessage LocationsCSV(string FilePath, Guid? LocationTypeKey = null, bool SkipGeocoding = false )
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

            var locationTypeService = new LocationTypeService();
            var locType = locationTypeService.GetLocationType(LocTypeKey);

            StatusMessage Msg = new StatusMessage();
            Msg.ObjectName = FilePath;
            var MsgDetails = new StringBuilder();

            string FullPath = Mindfly.Files.GetMappedPath(FilePath);

            DataTable tableCSV = ConvertCSVtoDataTable(FullPath, true);

            var iCounter = 0;

            int RowsTotal = tableCSV.Rows.Count;
            int RowsSuccess = 0;
            int RowsFailure = 0;
            int GeocodeCount = 0;
            bool NeedsMaintenance = false;

            if (SkipGeocoding)
            {
                GeocodeCount = MAX_GEOCODE + 1;
            }

            foreach (DataRow row in tableCSV.Rows)
            {
                iCounter++;

                string locName = row.Field<string>("LocationName");
                var rowImportStatus = ImportRow(row, locType, GeocodeCount, tableCSV.Columns, out GeocodeCount);

                Msg.InnerStatuses.Add(rowImportStatus);

                if (rowImportStatus.Success)
                {
                    RowsSuccess++;
                    if (StringExtensions.IsNullOrWhiteSpace(rowImportStatus.Code))
                    {
                        MsgDetails.AppendLine(string.Format("'{0}' was imported successfully.", locName));
                    }
                    else
                    {
                        MsgDetails.AppendLine(string.Format("'{0}' was imported with some issues: {1}", locName, rowImportStatus.Message));

                        if (rowImportStatus.Code.Contains("GeocodingProblem") || rowImportStatus.Code.Contains("UnableToUpdateDBGeography"))
                        {
                            NeedsMaintenance = true;
                        }
                    }

                }
                else
                {
                    MsgDetails.AppendLine(string.Format("Error importing '{0}' : {1}", locName, rowImportStatus.Message));
                    RowsFailure++;
                }

                if (rowImportStatus.RelatedException != null)
                {
                    LogHelper.Error(typeof(Import), string.Format("Error on '{0}'", locName), rowImportStatus.RelatedException);
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
            
            return Msg;
        }

        private static StatusMessage ImportRow(DataRow row, LocationType LocType, int GeocodeCount, DataColumnCollection ImportColumns, out int geocodeCountReturn)
        {
            string locName = row.Field<string>("LocationName");

            var ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = locName;
            ReturnMsg.Success = true;
            var Msg = new StringBuilder();
            var geocodeCountNew = GeocodeCount;

            //Create new Location for row
            var newLoc = new Location(locName, LocType.Key);
            Repositories.LocationRepo.Insert(newLoc);

            try
            {
                //Default Props
                var locationTypeService = new LocationTypeService();
                var defaultLocType = locationTypeService.GetLocationType(Constants.DefaultLocationTypeKey);
                foreach (var prop in defaultLocType.Properties)
                {
                    string colName = prop.Alias;
                    if (ImportColumns.Contains(colName))
                    {
                        newLoc.AddPropertyData(colName, row.Field<object>(colName));
                    }
                    else
                    {
                        newLoc.AddPropertyData(colName, null);
                        Msg.AppendLine(string.Concat("Data for '", colName, "' was not included in the import file."));
                    }
                }

                //Custom Props
                if (LocType.Key != defaultLocType.Key)
                {
                    foreach (var prop in LocType.Properties)
                    {
                        string colName = prop.Alias;

                        if (ImportColumns.Contains(colName))
                        {
                            newLoc.AddPropertyData(colName, row.Field<object>(colName));
                        }
                        else
                        {
                            newLoc.AddPropertyData(colName, null);
                            Msg.AppendLine(string.Concat("Data for '", colName, "' was not included in the import file."));
                        }
                    }
                }

                // SAVE properties of new location to db
                try
                {
                    Repositories.LocationRepo.Update(newLoc);
                }
                catch (Exception eSave)
                {
                    ReturnMsg.Success = false;
                    ReturnMsg.RelatedException = eSave;
                    ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",InsertError" : "InsertError";
                    Msg.AppendLine("There was a problem saving the new location data.");
                }

                //Check for Lat/Long values - import if present
                if (ImportColumns.Contains("Latitude"))
                {
                    int convertedInt = 0;
                    Int32.TryParse(row.Field<string>("Latitude"), out convertedInt);
                    newLoc.Latitude = convertedInt;
                }

                if (ImportColumns.Contains("Longitude"))
                {
                    int convertedInt = 0;
                    Int32.TryParse(row.Field<string>("Longitude"), out convertedInt);
                    newLoc.Longitude = convertedInt;
                }

                //If Lat/Long are both 0... attempt geocoding
                if (newLoc.Latitude == 0 && newLoc.Longitude == 0)
                {
                    //TODO: make dynamic based on provider limit
                    if (GeocodeCount >= MAX_GEOCODE)
                    {
                        ReturnMsg.Success = true;
                        ReturnMsg.Code = "GeocodingProblem";
                        Msg.AppendLine(
                            "This address exceeded the limits for geo-coding in a batch. Please run maintenance to update geo-codes later.");
                    }
                    else
                    {
                        try
                        {
                            var newCoordinate = DoGeocoding.GetCoordinateForAddress(newLoc.Address);
                            newLoc.Latitude = newCoordinate.Latitude;
                            newLoc.Longitude = newCoordinate.Longitude;

                            geocodeCountNew++;
                        }
                        catch (Exception e1)
                        {
                            ReturnMsg.Success = true;
                            ReturnMsg.RelatedException = e1;
                            ReturnMsg.Code = "GeocodingProblem";
                            Msg.AppendLine(
                                "There was a problem geo-coding the address. Please run maintenance to update geo-codes later.");
                            LogHelper.Error(
                                typeof(Import),
                                string.Format("Geo-coding error while importing '{0}'", ReturnMsg.ObjectName),
                                e1);
                        }
                    }
                }

                // SAVE properties of new location to db again
                try
                {
                    Repositories.LocationRepo.Update(newLoc);
                }
                catch (Exception eSave)
                {
                    ReturnMsg.Success = false;
                    ReturnMsg.RelatedException = eSave;
                    ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",InsertError" : "InsertError";
                    Msg.AppendLine("There was a problem saving the new location data.");
                }

                // UPDATE Geography DB field using Lat/Long
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
                catch (Exception eGeoDB)
                {
                    ReturnMsg.Success = true;
                    ReturnMsg.RelatedException = eGeoDB;
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
                LogHelper.Error(typeof(Import), string.Format("ImportRow: Error importing location '{0}'", locName), ex);
            }

            ReturnMsg.Message = Msg.ToString();

            geocodeCountReturn = geocodeCountNew;
            
            return ReturnMsg;
        }

        public static DataTable ConvertCSVtoDataTable(string path, bool hasHeader)
        {
            DataTable dt = new DataTable();

            using (var MyReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(path))
            {
                MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                MyReader.Delimiters = new String[] { "," };

                string[] currentRow;

                //'Loop through all of the fields in the file.  
                //'If any lines are corrupt, report an error and continue parsing.  
                bool firstRow = true;
                while (!MyReader.EndOfData)
                {
                    try
                    {
                        currentRow = MyReader.ReadFields();

                        //Add the header columns
                        if (hasHeader && firstRow)
                        {
                            foreach (string c in currentRow)
                            {
                                dt.Columns.Add(c, typeof(string));
                            }

                            firstRow = false;
                            continue;
                        }

                        //Create a new row
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);

                        //Loop thru the current line and fill the data out
                        for (int c = 0; c < currentRow.Count(); c++)
                        {
                            dr[c] = currentRow[c];
                        }
                    }
                    catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
                    {
                        //Handle the exception here
                    }
                }
            }

            return dt;
        }

        private static DataTable ConvertCSVtoDataTable1(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = sr.ReadLine().Split(',');
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        #region OLD

        //public static StatusMessage LocationsCSV(string FilePath, Guid? LocationTypeKey = null)
        //{
        //    Guid LocTypeKey;
        //    if (LocationTypeKey == null)
        //    {
        //        LocTypeKey = Constants.DefaultLocationTypeKey;
        //    }
        //    else
        //    {
        //        LocTypeKey = (Guid)LocationTypeKey;
        //    }

        //    StatusMessage Msg = new StatusMessage();
        //    Msg.ObjectName = FilePath;
        //    var MsgDetails = new StringBuilder();

        //    string FullPath = Mindfly.Files.GetMappedPath(FilePath);

        //    try
        //    {
        //        // Using http://filehelpers.sourceforge.net/ to process CSV files 

        //        // Dynamic class definition
        //        string dynamicClassDef = DynamicLocationFlat.GetDynamicClass(LocTypeKey);

        //        Type t = ClassBuilder.ClassFromString(dynamicClassDef);

        //        FileHelperEngine fhEngine = new FileHelperEngine(t);

        //        //DataTable = engine.ReadFileAsDT("test.txt");

        //        //FileHelperEngine fhEngine = new FileHelperEngine(typeof(LocationFlat));               

        //        //LocationFlat[] csvLocationFlats = fhEngine.ReadFile(FullPath) as LocationFlat[];
        //        var csvLocationFlats = fhEngine.ReadFile(FullPath);

        //        int RowsTotal = csvLocationFlats.Count();
        //        int RowsSuccess = 0;
        //        int RowsFailure = 0;
        //        int GeocodeCount = 0;
        //        bool NeedsMaintenance = false;

        //        foreach (dynamic row in csvLocationFlats)
        //        {
        //            if (NeedsGeocoding(row))
        //            {
        //                GeocodeCount++;
        //            }

        //            var RowStatusMsg = ImportItem(row, LocTypeKey, GeocodeCount);
        //            if (RowStatusMsg.Success)
        //            {
        //                RowsSuccess++;
        //                if (StringExtensions.IsNullOrWhiteSpace(RowStatusMsg.Code))
        //                {
        //                    MsgDetails.AppendLine(string.Format("'{0}' was imported successfully.", row.LocationName));
        //                }
        //                else
        //                {
        //                    MsgDetails.AppendLine(string.Format("'{0}' was imported with some issues: {1}", row.LocationName, RowStatusMsg.Message));

        //                    if (RowStatusMsg.Code.Contains("GeocodingProblem") || RowStatusMsg.Code.Contains("UnableToUpdateDBGeography"))
        //                    {
        //                        NeedsMaintenance = true;
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                MsgDetails.AppendLine(string.Format("Error importing '{0}' : {1}", row.LocationName, RowStatusMsg.Message));
        //                RowsFailure++;
        //            }

        //            if (RowStatusMsg.RelatedException != null)
        //            {
        //                LogHelper.Error(typeof(Import), string.Format("Error on '{0}'", row.LocationName), RowStatusMsg.RelatedException);
        //            }
        //        }


        //        //Compile final status message
        //        Msg.Message += string.Format(
        //            "Out of {0} total locations, {1} were imported successfully and {2} failed.",
        //            RowsTotal,
        //            RowsSuccess,
        //            RowsFailure);

        //        if (NeedsMaintenance)
        //        {
        //            Msg.Message += " Some rows encountered issues which might be fixed by running some maintenance tasks.";
        //        }

        //        Msg.MessageDetails = MsgDetails.ToString();
        //        Msg.Success = true;
        //        LogHelper.Info(typeof(Import), string.Format("Final Status importing file '{0}': {1} : {2}", FilePath, Msg.Message, Msg.MessageDetails));
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("found after the last field"))
        //        {
        //            Msg.Message = "The imported file data doesn't match the correct format. Please check that your columns are defined correctly.";
        //            Msg.Success = false;
        //            Msg.Code = ex.GetType().ToString();
        //            Msg.RelatedException = ex;
        //            LogHelper.Error(typeof(Import), string.Format("Error importing file '{0}'", FilePath), ex);
        //        }
        //        else if (ex.Message.Contains("The NullValue is of type: String"))
        //        {
        //            Msg.Success = false;
        //            Msg.Code = ex.GetType().ToString();
        //            Msg.RelatedException = ex;
        //            if (ex.Message.Contains("Longitude") || ex.Message.Contains("Latitude"))
        //            {
        //                Msg.Message = "Please make sure that you have zeros in the blank fields for 'Longitude' and 'Latitude'.";
        //            }
        //            else
        //            {
        //                Msg.Message = "Please make sure that you have zeros in the blank fields for any numeric columns.";
        //            }

        //            LogHelper.Error(typeof(Import), string.Format("Error importing file '{0}'", FilePath), ex);
        //        }
        //        else
        //        {
        //            Msg.Message = string.Format("An error occurred : {0}", ex.Message);
        //            Msg.Success = false;
        //            Msg.Code = ex.GetType().ToString();
        //            Msg.RelatedException = ex;
        //            LogHelper.Error(typeof(Import), string.Format("Error importing file '{0}'", FilePath), ex);
        //        }
        //    }

        //    return Msg;
        //}

        //private static StatusMessage ImportItem(dynamic locFlat, Guid LocationTypeKey, int GeocodeCount)
        //{
        //    var ReturnMsg = new StatusMessage();
        //    ReturnMsg.ObjectName = locFlat.LocationName;
        //    ReturnMsg.Success = true;
        //    var Msg = new StringBuilder();

        //    try
        //    {
        //        //Create new Location
        //        var newLoc = new Location(locFlat.LocationName, LocationTypeKey);

        //        //var newKey = newLoc.Key;
        //        Repositories.LocationRepo.Insert(newLoc);
        //        //newLoc = Repositories.LocationRepo.GetByKey(newKey);

        //        //Set default properties
        //        newLoc.AddPropertyData("Address1", locFlat.Address1);
        //        newLoc.AddPropertyData("Address2", locFlat.Address2);
        //        newLoc.AddPropertyData("Locality", locFlat.Locality);
        //        newLoc.AddPropertyData("Region", locFlat.Region);
        //        newLoc.AddPropertyData("PostalCode", locFlat.PostalCode);
        //        newLoc.AddPropertyData("CountryCode", locFlat.CountryCode);
        //        newLoc.AddPropertyData("PhoneNumber", locFlat.PhoneNumber);
        //        newLoc.AddPropertyData("Email", locFlat.Email);

        //        if (LocationTypeKey != Constants.DefaultLocationTypeKey)
        //        {
        //            var CustomLtProps = Repositories.LocationTypePropertyRepo.GetByLocationType(LocationTypeKey);

        //            //We need to copy the properties into their own list 
        //            //to avoid the "Collection was modified; enumeration operation may not execute." error
        //            var propsCollection = new List<LocationTypeProperty>();

        //            foreach (var customProp in CustomLtProps)
        //            {
        //                propsCollection.Add(customProp);
        //            }

        //            foreach (var prop in propsCollection)
        //            {
        //                var importValue = locFlat.GetProperty(prop.Alias);

        //                if (prop.DataType.PropertyEditorAlias == "Umbraco.DropDown")
        //                {
        //                    //convert to prevalue node Id
        //                    var idValue = DataValuesHelper.GetPreValueId(prop.DataTypeId, importValue);
        //                    newLoc.AddPropertyData(prop.Alias, idValue);
        //                }
        //                ////TODO: Add conversion checks for Boolean values as well (Yes/No, True/False, data/Null) to convert to 1/0
        //                else
        //                {
        //                    //Just import the data as-is
        //                    newLoc.AddPropertyData(prop.Alias, importValue);
        //                }
        //            }
        //        }

        //        //Attempt geocoding
        //        if (NeedsGeocoding(locFlat))
        //        {
        //            if (GeocodeCount >= 400)
        //            {
        //                ReturnMsg.Success = true;
        //                ReturnMsg.Code = "GeocodingProblem";
        //                Msg.AppendLine("This address exceeded the limits for geo-coding in a batch.");
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    var locAddress = new Address()
        //                    {
        //                        Address1 = locFlat.Address1,
        //                        Address2 = locFlat.Address2,
        //                        Locality = locFlat.Locality,
        //                        Region = locFlat.Region,
        //                        PostalCode = locFlat.PostalCode,
        //                        CountryCode = locFlat.CountryCode
        //                    };

        //                    //TODO: make dynamic based on provider limit
        //                    var newCoordinate = DoGeocoding.GetCoordinateForAddress(locAddress);

        //                    newLoc.Latitude = newCoordinate.Latitude;
        //                    newLoc.Longitude = newCoordinate.Longitude;
        //                }
        //                catch (Exception e1)
        //                {
        //                    ReturnMsg.Success = true;
        //                    ReturnMsg.RelatedException = e1;
        //                    ReturnMsg.Code = "GeocodingProblem";
        //                    Msg.AppendLine("There was a problem geo-coding the address.");
        //                    LogHelper.Error(typeof(Import), string.Format("Geo-coding error while importing '{0}'", ReturnMsg.ObjectName), e1);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            newLoc.Latitude = locFlat.Latitude;
        //            newLoc.Longitude = locFlat.Longitude;
        //        }


        //        // SAVE properties of new location to db
        //        try
        //        {
        //            //Repositories.LocationRepo.Insert(newLoc);
        //            Repositories.LocationRepo.Update(newLoc);
        //        }
        //        catch (Exception e2)
        //        {
        //            ReturnMsg.Success = false;
        //            ReturnMsg.RelatedException = e2;
        //            ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",InsertError" : "InsertError";
        //            Msg.AppendLine("There was a problem saving the new location data.");
        //        }

        //        // UPDATE Geography field using Lat/Long
        //        try
        //        {
        //            if (newLoc.Latitude != 0 && newLoc.Longitude != 0)
        //            {
        //                Repositories.LocationRepo.UpdateDbGeography(newLoc);
        //            }
        //            else
        //            {
        //                newLoc.DbGeogNeedsUpdated = true;
        //                Repositories.LocationRepo.Update(newLoc);
        //            }
        //        }
        //        catch (Exception e3)
        //        {
        //            ReturnMsg.Success = true;
        //            ReturnMsg.RelatedException = e3;
        //            ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",UnableToUpdateDBGeography" : "UnableToUpdateDBGeography";
        //            Msg.AppendLine("Unable to update the coordinates in the database.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ReturnMsg.Success = false;
        //        ReturnMsg.RelatedException = ex;
        //        ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",UnknownException" : "UnknownException";
        //        Msg.AppendLine(ex.Message);
        //        LogHelper.Error(typeof(Import), string.Format("ImportItem: Error importing location '{0}'", locFlat.LocationName), ex);
        //    }

        //    ReturnMsg.Message = Msg.ToString();
        //    return ReturnMsg;
        //}

        #endregion

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
