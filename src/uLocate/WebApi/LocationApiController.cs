namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using uLocate.Configuration;
    using uLocate.Data;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The location type api controller for use by the umbraco back-office
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class LocationApiController : UmbracoAuthorizedApiController
    {
        ///// /umbraco/backoffice/uLocate/LocationApi/Test
        //[System.Web.Http.AcceptVerbs("GET")]
        //public bool Test()
        //{
        //    return true;
        //}

        /// <summary>
        /// Create a new Location of type "default"
        /// /umbraco/backoffice/uLocate/LocationApi/Create?LocationName=xxx
        /// </summary>
        /// <param name="LocationName">
        /// The location name.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/> of the newly created Location
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public Guid Create(string LocationName)
        {
            return uLocate.Helpers.Persistence.CreateLocation(LocationName);
        }

        /// <summary>
        /// Create a new Location 
        /// /umbraco/backoffice/uLocate/LocationApi/Create?LocationName=xxx&LocationTypeGuid=xxx
        /// </summary>
        /// <param name="LocationName">
        /// The location name.
        /// </param>
        /// <param name="LocationTypeGuid">
        /// The guid key for the Location Type.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/> of the newly created Location
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public Guid Create(string LocationName, Guid LocationTypeGuid)
        {
            return uLocate.Helpers.Persistence.CreateLocation(LocationName, LocationTypeGuid);
        }

        /// <summary>
        /// Update a location 
        /// /umbraco/backoffice/uLocate/LocationApi/Update
        /// </summary>
        /// <param name="UpdatedLocation">
        /// The updated location object
        /// </param>
        /// <returns>
        /// The <see cref="LocationType"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public JsonLocation Update(JsonLocation UpdatedLocation)
        {
            var Result = uLocate.Helpers.Persistence.UpdateLocation(UpdatedLocation.ConvertToLocation());

            return new JsonLocation(Result);
        }

        /// <summary>
        /// Get a location by its Key
        /// /umbraco/backoffice/uLocate/LocationApi/GetByKey
        /// </summary>
        /// <param name="Key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Location"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public JsonLocation GetByKey(Guid Key)
        {
            var Result = Repositories.LocationRepo.GetByKey(Key);

            return new JsonLocation(Result);
        }

        /// <summary>
        /// Delete a location 
        /// /umbraco/backoffice/uLocate/LocationApi/Delete
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
            var Result = Repositories.LocationRepo.Delete(Key);

            return Result;
        }

        /// <summary>
        /// Get all locations as a list
        /// /umbraco/backoffice/uLocate/LocationApi/GetAll
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<JsonLocation> GetAll()
        {
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetAll());

            return Result;
        }

        /// <summary>
        /// Get all locations as a paged list
        /// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5
        /// </summary>
        /// <param name="pageNum">
        /// The page num.
        /// </param>
        /// <param name="itemsPerPage">
        /// The items per page.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PagedLocations GetAllPaged(long pageNum, long itemsPerPage)
        {
            var paged = Repositories.LocationRepo.GetPaged(pageNum + 1, itemsPerPage, string.Empty);
            var result = new PagedLocations() 
                            {
                                 Locations = paged,
                                 PageNum = pageNum,
                                 ItemsPerPage = itemsPerPage,
                                 TotalItems = paged.Count
                             };

            return result;
        }

        /// <summary>
        /// Gets an empty json location.
        /// /umbraco/backoffice/uLocate/LocationApi/GetEmptyJsonLocation
        /// </summary>
        /// <returns>
        /// An empty <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public JsonLocation GetEmptyJsonLocation()
        {
            var Result = new JsonLocation();
            var EmptyProp = new JsonPropertyData();
            Result.CustomPropertyData.Add(EmptyProp);

            return Result;
        }

        /// <summary>
        /// Updates Lat/Long coordinates for all Locations which require it.
        /// /umbraco/backoffice/uLocate/LocationApi/UpdateCoordinatesAsNeeded
        /// </summary>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public StatusMessage UpdateCoordinatesAsNeeded()
        {
            var Result = Repositories.LocationRepo.UpdateGeoForAllNeeded();

            return Result;
        }

    }
}

