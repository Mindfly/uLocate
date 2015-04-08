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
        #region Location-Related

        public static Location GetLocation(Guid LocationKey)
        {
            var Result = Repositories.LocationRepo.GetByKey(LocationKey);

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
    }
}
