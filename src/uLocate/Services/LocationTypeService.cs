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
    public class LocationTypeService
    {
        #region LocationType-Related

        public LocationType GetLocationType(Guid LocationTypeKey)
        {
            var Result = Repositories.LocationTypeRepo.GetByKey(LocationTypeKey);

            return Result;
        }

        public LocationType GetLocationTypeByName(string LocationTypeName)
        {
            var result = Repositories.LocationTypeRepo.GetByName(LocationTypeName).FirstOrDefault();

            return result;
        }

        #endregion

    }


}
