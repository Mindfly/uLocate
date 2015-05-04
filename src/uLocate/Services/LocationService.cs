namespace uLocate.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Models;
    using uLocate.Persistance;

    /// <summary>
    /// Functions related to looking up and editing data which can be called from Api Controllers etc.
    /// </summary>
    public class LocationService
    {
        #region Location-Related

        public Location GetLocation(Guid LocationKey)
        {
            var Result = Repositories.LocationRepo.GetByKey(LocationKey);

            return Result;
        }

        public StatusMessage DeleteLocation(Guid LocationKey)
        {
            var Result = Repositories.LocationRepo.Delete(LocationKey);

            return Result;
        }

        public Guid CreateLocation(string LocationName, bool UpdateIfFound = false)
        {
            var Result = CreateLocation(LocationName, uLocate.Constants.DefaultLocationTypeKey, UpdateIfFound);

            return Result;
        }

        public Guid CreateLocation(string LocationName, Guid LocationTypeGuid, bool UpdateIfFound = false)
        {
            bool DoUpdate = false;
            Guid LocType;

            if (LocationTypeGuid != Guid.Empty)
            {
                LocType = LocationTypeGuid;
            }
            else
            {
                LocType = uLocate.Constants.DefaultLocationTypeKey;
            }

            if (UpdateIfFound)
            {
                //Lookup first
                var matchingLocations = Repositories.LocationRepo.GetByName(LocationName);
                if (matchingLocations.Any())
                {
                    Location lookupLoc = matchingLocations.FirstOrDefault();
                    lookupLoc.Name = LocationName;
                    lookupLoc.LocationTypeKey = LocType;
                    Repositories.LocationRepo.Update(lookupLoc);
                    return lookupLoc.Key;
                }
            }

            Location newLoc = new Location(LocationName, LocType);
            Repositories.LocationRepo.Insert(newLoc);
            return newLoc.Key;
        }

        public Location UpdateLocation(Location UpdatedLocation)
        {
            Repositories.LocationRepo.Update(UpdatedLocation);

            var Result = Repositories.LocationRepo.GetByKey(UpdatedLocation.Key);

            return Result;
        }

        public StatusMessage UpdateGeographyData(Location LocationToUpdate)
        {
            var returnMsg = new StatusMessage();

            try
            {
                Repositories.LocationRepo.UpdateLatLong(LocationToUpdate);
            }
            catch (Exception exLatLong)
            {
                returnMsg.Success = false;
                returnMsg.Code = "ErrorGeoCode";
                returnMsg.Message = string.Format("{0} location was unable to be updated. Error while geo-coding.", LocationToUpdate.Name);
                returnMsg.RelatedException = exLatLong;
            }

            try
            {
                Repositories.LocationRepo.UpdateDbGeography(LocationToUpdate);
            }
            catch (Exception eDb)
            {
                returnMsg.Success = false;
                returnMsg.Code = "ErrorDbGeography";
                returnMsg.Message = string.Format("{0} location was unable to be updated. Error while updating database geography.", LocationToUpdate.Name);
                returnMsg.RelatedException = eDb;
            }

            returnMsg.Success = true;
            returnMsg.Message = string.Format("{0} location was updated.", LocationToUpdate.Name);

            return returnMsg;
        }

        #endregion

        #region Location Collections

        public IEnumerable<Location> GetLocations()
        {
            var result = Repositories.LocationRepo.GetAll();

            return result;
        }

        public IEnumerable<Location> GetLocations(Guid LocationTypeKey)
        {
            var result = Repositories.LocationRepo.GetByType(LocationTypeKey);

            return result;
        }

        public IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, string Value)
        {
            var AllLocations = Repositories.LocationRepo.GetAll();
            
            var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value);

            return result;
        }

        public IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, int Value)
        {
            var AllLocations = Repositories.LocationRepo.GetAll();

            var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            return result;
        }

        public IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, DateTime Value)
        {
            var AllLocations = Repositories.LocationRepo.GetAll();

            var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            return result;
        }

        #endregion
    }
}
