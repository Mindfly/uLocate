namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using uLocate.Data;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The location type api controller for use by the umbraco back-office
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class LocationTypeApiController : UmbracoAuthorizedApiController
    {
        /// /umbraco/backoffice/uLocate/LocationTypeApi/Test
        [System.Web.Http.AcceptVerbs("GET")]
        public bool Test()
        {
            return true;
        }

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
            LocationType newLocType = new LocationType();
            newLocType.Name = LocationTypeName;
            Repositories.LocationTypeRepo.Insert(newLocType);

            //var Result = Repositories.LocationTypeRepo.GetByKey(newLocType.Key);
            var Result = newLocType.Key;

            return Result;
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
            ////First, do properties
            //foreach (var jsonProp in UpdatedLocationTypeJson.Properties)
            //{
            //    LocationTypeProperty UpdatedProp = jsonProp.ConvertToLocationTypeProperty();
            //}

            //then do LT
            LocationType UpdatedLocationType = UpdatedLocationTypeJson.ConvertToLocationType();

            Repositories.LocationTypeRepo.Update(UpdatedLocationType);

            var FullResult = Repositories.LocationTypeRepo.GetByKey(UpdatedLocationType.Key);

            var JsonResult = new JsonLocationType(FullResult);

            return JsonResult;
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
        /// The <see cref="LocationType"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public LocationType GetByKey(Guid Key)
        {
            var Result = Repositories.LocationTypeRepo.GetByKey(Key);

            return Result;
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
            var Result = Repositories.LocationTypeRepo.Delete(Key, true);

            return Result;
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
            var locationTypes = Repositories.LocationTypeRepo.GetAll().ToList();

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
            var Result = Repositories.LocationTypePropertyRepo.GetByKey(Key);

            return Result;
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
            var props = Repositories.LocationTypePropertyRepo.GetAll().ToList();

            //var returnList = new List<JsonLocationType>();

            //foreach (var loc in locationTypes)
            //{
            //    returnList.Add(new JsonLocationType(loc));
            //}

            return props;
        } 

        #endregion
    }
}

