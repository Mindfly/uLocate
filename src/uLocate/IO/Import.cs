namespace uLocate.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Configuration;
    using System.Text;
    using System.Threading.Tasks;
    using FileHelpers;

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

            FileHelperEngine fhEngine = new FileHelperEngine(typeof(LocationFlat));

            string FullPath = Mindfly.Files.GetMappedPath(FilePath);
            var MsgDetails = new StringBuilder();

            try
            {
                LocationFlat[] csvLocationFlats = fhEngine.ReadFile(FullPath) as LocationFlat[];

                int RowsTotal = csvLocationFlats.Count();
                int RowsSuccess = 0;
                int RowsFailure = 0;
                int GeocodeCount = 0;
                bool NeedsMaintenance = false;


                foreach (LocationFlat row in csvLocationFlats)
                {
                    if (row.NeedsGeocoding())
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

        /// <summary>
        /// Test whether this data needs to be geo-coded.
        /// </summary>
        /// <param name="locFlat">
        /// The location flat record
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private static bool NeedsGeocoding(this LocationFlat locFlat)
        {
            bool Result = true;
            if (locFlat.Latitude != 0 && locFlat.Longitude != 0)
            {
                Result = false;
            }

            return Result;
        }

        private static StatusMessage ImportItem(LocationFlat locFlat, Guid LocationTypeKey, int GeocodeCount)
        {
            var ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = locFlat.LocationName;
            ReturnMsg.Success = true;
            var Msg = new StringBuilder();

            try
            {
                //Create new Location
                var newLoc = new Location()
                {
                    Name = locFlat.LocationName
                };

                //Attempt geocoding
                if (locFlat.NeedsGeocoding())
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

                //set properties
                newLoc.AddPropertyData("Address1", locFlat.Address1);
                newLoc.AddPropertyData("Address2", locFlat.Address2);
                newLoc.AddPropertyData("Locality", locFlat.Locality);
                newLoc.AddPropertyData("Region", locFlat.Region);
                newLoc.AddPropertyData("PostalCode", locFlat.PostalCode);
                newLoc.AddPropertyData("CountryCode", locFlat.CountryCode);
                newLoc.AddPropertyData("PhoneNumber", locFlat.PhoneNumber);
                newLoc.AddPropertyData("Email", locFlat.Email);

                //TODO: Support additional custom properties

                try
                {
                    Repositories.LocationRepo.Insert(newLoc);
                }
                catch (Exception e2)
                {
                    ReturnMsg.Success = false;
                    ReturnMsg.RelatedException = e2;
                    ReturnMsg.Code = ReturnMsg.Code != "" ? ReturnMsg.Code + ",InsertError" : "InsertError";
                    Msg.AppendLine("There was a problem saving the new location data.");
                }

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
    }
}
