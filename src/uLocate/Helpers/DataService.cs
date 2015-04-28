using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Helpers
{
    using uLocate.Models;
    using uLocate.Persistance;

    /// <summary>
    /// Functions related to looking up and editing data which can be called from Api Controllers etc.
    /// </summary>
    public static class DataService
    {
        #region LocationType-Related

        public static LocationType GetLocationType(Guid LocationTypeKey)
        {
            var Result = Repositories.LocationTypeRepo.GetByKey(LocationTypeKey);

            return Result;
        }

        public static LocationType GetLocationTypeByName(string LocationTypeName)
        {
            var result = Repositories.LocationTypeRepo.GetByName(LocationTypeName).FirstOrDefault();

            return result;
        }

        #endregion

        #region Location-Related

        public static Location GetLocation(Guid LocationKey)
        {
            var Result = Repositories.LocationRepo.GetByKey(LocationKey);

            return Result;
        }

        public static StatusMessage DeleteLocation(Guid LocationKey)
        {
            var Result = Repositories.LocationRepo.Delete(LocationKey);

            return Result;
        }

        public static Guid CreateLocation(string LocationName, bool UpdateIfFound = false)
        {
            var Result = CreateLocation(LocationName, uLocate.Constants.DefaultLocationTypeKey, UpdateIfFound);

            return Result;
        }

        public static Guid CreateLocation(string LocationName, Guid LocationTypeGuid, bool UpdateIfFound = false)
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

        public static Location UpdateLocation(Location UpdatedLocation, bool UpdateIfFound = false)
        {
            Repositories.LocationRepo.Update(UpdatedLocation);

            var Result = Repositories.LocationRepo.GetByKey(UpdatedLocation.Key);

            return Result;
        }

        #endregion

        #region Location Collections

        public static IEnumerable<Location> GetLocations()
        {
            var result = Repositories.LocationRepo.GetAll();

            return result;
        }

        public static IEnumerable<Location> GetLocations(Guid LocationTypeKey)
        {
            var result = Repositories.LocationRepo.GetByType(LocationTypeKey);

            return result;
        }

        public static IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, string Value)
        {
            var AllLocations = Repositories.LocationRepo.GetAll();
            
            var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value);

            return result;
        }

        public static IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, int Value)
        {
            var AllLocations = Repositories.LocationRepo.GetAll();

            var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            return result;
        }

        public static IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, DateTime Value)
        {
            var AllLocations = Repositories.LocationRepo.GetAll();

            var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            return result;
        }

        #endregion
    }

    
}
