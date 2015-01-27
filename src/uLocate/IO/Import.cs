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

    using Umbraco.Core.Logging;

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

            try
            {
                LocationFlat[] csvLocationFlats = fhEngine.ReadFile(FullPath) as LocationFlat[];

                int RowsTotal= csvLocationFlats.Count();
                int RowsSuccess = 0;
                int RowsFailure = 0;
                int GeocodeCount = 0;
                

                foreach (LocationFlat row in csvLocationFlats)
                {
                    if (row.NeedsGeocoding())
                    {
                        GeocodeCount++;
                    }

                    bool RowSuccess = ImportItem(row, LocTypeKey, GeocodeCount);
                    if (RowSuccess)
                    {
                        Msg.Message += Environment.NewLine + string.Format("'{0}' imported successfully.", row.LocationName);
                        RowsSuccess++;
                    }
                    else
                    {
                        Msg.Message += Environment.NewLine + string.Format("Error importing '{0}'.", row.LocationName);
                        RowsFailure++;
                    }
                }

                Msg.Message += Environment.NewLine + string.Format( 
                    "Out of {0} total locations, {1} were imported successfully and {2} failed.", 
                    RowsTotal, 
                    RowsSuccess, 
                    RowsFailure);
                Msg.Success = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("found after the last field"))
                {
                    Msg.Message = "The imported file data doesn't match the correct format. Please check that your columns are defined correctly.";
                    Msg.Success = false;
                    Msg.Code = ex.GetType().ToString();
                    Msg.RelatedException = ex;
                    LogHelper.Error(typeof(Import), string.Format("Error importing file '{0}'", FilePath), ex);
                }
                else
                {
                    Msg.Message = "An error occurred";
                    Msg.Success = false;
                    Msg.Code = ex.GetType().ToString();
                    Msg.RelatedException = ex;
                    LogHelper.Error(typeof(Import), string.Format("Error importing file '{0}'", FilePath), ex);
                }

            }

            return Msg;
        }

        private static bool NeedsGeocoding(this LocationFlat locFlat)
        {
            bool Result = true;
            if (locFlat.Latitude != 0 && locFlat.Longitude != 0)
            {
                Result = false;
            }

            return Result;
        }

        private static bool ImportItem(LocationFlat locFlat, Guid LocationTypeKey, int GeocodeCount)
        {
            bool Success = true;

            try
            {
                var newLoc = new Location()
                {
                    Name = locFlat.LocationName
                };

                //TODO: make dynamic based on provider limit
                if (locFlat.NeedsGeocoding() && GeocodeCount < 400)
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
                    var newCoordinate = DoGeocoding.GetCoordinateForAddress(locAddress);

                    newLoc.Latitude = newCoordinate.Latitude;
                    newLoc.Longitude = newCoordinate.Longitude;
                }
                else
                {
                    newLoc.Latitude = locFlat.Latitude;
                    newLoc.Longitude = locFlat.Longitude;
                }

                newLoc.AddPropertyData("Address1", locFlat.Address1);
                newLoc.AddPropertyData("Address2", locFlat.Address2);
                newLoc.AddPropertyData("Locality", locFlat.Locality);
                newLoc.AddPropertyData("Region", locFlat.Region);
                newLoc.AddPropertyData("PostalCode", locFlat.PostalCode);
                newLoc.AddPropertyData("CountryCode", locFlat.CountryCode);
                newLoc.AddPropertyData("PhoneNumber", locFlat.PhoneNumber);
                newLoc.AddPropertyData("Email", locFlat.Email);

                Repositories.LocationRepo.Insert(newLoc);

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
            catch (Exception ex)
            {
                LogHelper.Error(typeof(Import), string.Format("ImportItem: Error importing location '{0}'", locFlat.LocationName), ex);
                Success = false;
            }

            return Success;
        }
    }
}
