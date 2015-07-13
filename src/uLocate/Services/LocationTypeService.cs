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
        #region CUD

        public LocationType Update(LocationType UpdatedLocationType)
        {
            Repositories.LocationTypeRepo.Update(UpdatedLocationType);

            var result = Repositories.LocationTypeRepo.GetByKey(UpdatedLocationType.Key);

            Repositories.LocationRepo.UpdateWithNewProps(result.Key);

            return result;
        }


        public StatusMessage Delete(Guid LocationTypeKey)
        {
            var result = Repositories.LocationTypeRepo.Delete(LocationTypeKey, true);

            return result;
        }

        #endregion

        #region Querying

        public LocationType GetLocationType(Guid LocationTypeKey)
        {
            var Result = Repositories.LocationTypeRepo.GetByKey(LocationTypeKey);

            return Result;
        }

        public LocationType GetLocationType(string LocationTypeName)
        {
            var result = Repositories.LocationTypeRepo.GetByName(LocationTypeName).FirstOrDefault();

            return result;
        }

        public List<LocationType> GetLocationTypes()
        {
            var locationTypes = Repositories.LocationTypeRepo.GetAll().ToList();

            return locationTypes;
        }

        public LocationTypeProperty GetProperty(Guid PropertyKey)
        {
           return Repositories.LocationTypePropertyRepo.GetByKey(PropertyKey);
        }

        public List<LocationTypeProperty> GetProperties()
        {
            return Repositories.LocationTypePropertyRepo.GetAll().ToList();
        }


        #endregion
    }


}
