namespace uLocate.UI.WebApi
{
    using System;
    using System.Collections.Generic;

    using uLocate.Models;
    using uLocate.Services;

    using Umbraco.Web.WebApi;

    /// <summary>
    /// The location type api controller for use by the umbraco back-office
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class LocationTypeApiController : UmbracoAuthorizedApiController
    {
        private LocationService locationService = new LocationService();
        private LocationTypeService locationTypeService = new LocationTypeService();

        /// /umbraco/backoffice/uLocate/LocationTypeApi/Test
        //[System.Web.Http.AcceptVerbs("GET")]
        //public bool Test()
        //{
        //    return true;
        //}

        #region Location Types

        /// <summary>
        /// Create a new Location Type
        /// /umbraco/backoffice/uLocate/LocationTypeApi/Create?LocationTypeName="xxx"
        /// </summary>
        /// <param name="LocationTypeName">
        /// The location type name.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/> of the newly created LocationType
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public Guid Create(string LocationTypeName)
        {
            //OLD
            //LocationType newLocType = new LocationType();
            //newLocType.Name = LocationTypeName;
            //Repositories.LocationTypeRepo.Insert(newLocType);

            //var Result = Repositories.LocationTypeRepo.GetByKey(newLocType.Key);
            var result = locationService.CreateLocation(LocationTypeName);

            return result;
        }

        /// <summary>
        /// Update a location type
        /// /umbraco/backoffice/uLocate/LocationTypeApi/Update
        /// </summary>
        /// <param name="UpdatedLocationTypeJson">
        /// The Updated Location Type Json.
        /// </param>
        /// <returns>
        /// The <see cref="JsonLocationType"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public JsonLocationType Update(JsonLocationType UpdatedLocationTypeJson)
        {
            LocationType updatedLocationType = UpdatedLocationTypeJson.ConvertToLocationType();

            var fullResult = locationTypeService.Update(updatedLocationType);

            var jsonResult = new JsonLocationType(fullResult);

            return jsonResult;
        }

        /// <summary>
        /// Delete a location type
        /// /umbraco/backoffice/uLocate/LocationTypeApi/Delete
        /// </summary>
        /// <param name="Key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public StatusMessage Delete(Guid Key)
        {
            var result = locationTypeService.Delete(Key);

            return result;
        }

        /// <summary>
        /// Gets an empty json location type.
        /// /umbraco/backoffice/uLocate/LocationTypeApi/GetEmptyJsonLocationType
        /// </summary>
        /// <returns>
        /// An empty <see cref="JsonLocationType"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public JsonLocationType GetEmptyJsonLocationType()
        {
            var Result = new JsonLocationType();
            var EmptyProp = new JsonTypeProperty();
            Result.Properties.Add(EmptyProp);

            return Result;
        }

        /// <summary>
        /// Get a location type by its Key
        /// /umbraco/backoffice/uLocate/LocationTypeApi/GetByKey
        /// </summary>
        /// <param name="Key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="JsonLocationType"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public JsonLocationType GetByKey(Guid Key)
        {
            var loc = locationTypeService.GetLocationType(Key);

            var result = new JsonLocationType(loc);

            return result;
        }
        

        /// <summary>
        /// Get all LocationTypes in the system as a List
        /// /umbraco/backoffice/uLocate/LocationTypeApi/GetAll
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public List<JsonLocationType> GetAll()
        {
            var locationTypes = locationTypeService.GetLocationTypes();

            var returnList = new List<JsonLocationType>();

            foreach (var loc in locationTypes)
            {
                returnList.Add(new JsonLocationType(loc));
            }

            return returnList;
        } 

        #endregion

        #region Location Type Properties

        /// <summary>
        /// Get a LocationType Property by key.
        /// /umbraco/backoffice/uLocate/LocationTypeApi/GetPropertyByKey?Key=xxx
        /// </summary>
        /// <param name="Key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="LocationTypeProperty"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public LocationTypeProperty GetPropertyByKey(Guid Key)
        {
            var result = locationTypeService.GetProperty(Key);

            return result;
        }

        /// <summary>
        /// Get all properties in the database
        /// /umbraco/backoffice/uLocate/LocationTypeApi/GetAllProperties
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public List<LocationTypeProperty> GetAllProperties()
        {
            var props = locationTypeService.GetProperties();

            return props;
        } 

        #endregion
    }
}

