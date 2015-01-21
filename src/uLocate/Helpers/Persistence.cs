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
    /// Functions related to persisting data which can be called from Api Controllers etc.
    /// </summary>
    internal static class Persistence
    {
        #region Location-Related

        public static Guid CreateLocation(string LocationName, bool UpdateIfFound = false)
        {
            var Result = CreateLocation(LocationName, uLocate.Constants.DefaultLocationTypeKey, UpdateIfFound);

            return Result;
        }

        public static Guid CreateLocation(string LocationName, Guid LocationTypeGuid, bool UpdateIfFound = false)
        {
            Location newLoc = new Location();
            bool DoUpdate = false;

            if (UpdateIfFound)
            {
                //Lookup first
                var matchingLocations = Repositories.LocationRepo.GetByName(LocationName);
                if (matchingLocations.Any())
                {
                    newLoc = matchingLocations.FirstOrDefault();
                    DoUpdate = true;
                }
            }
            
            newLoc.Name = LocationName;

            if (LocationTypeGuid != Guid.Empty)
            {
                newLoc.LocationTypeKey = LocationTypeGuid;
            }
            else
            {
                newLoc.LocationTypeKey = uLocate.Constants.DefaultLocationTypeKey;
            }

            if (DoUpdate)
            {
                Repositories.LocationRepo.Update(newLoc);
            }
            else
            {
                Repositories.LocationRepo.Insert(newLoc);
            }
            

            //var Result = Repositories.LocationTypeRepo.GetByKey(newLocType.Key);
            var Result = newLoc.Key;

            return Result;
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
