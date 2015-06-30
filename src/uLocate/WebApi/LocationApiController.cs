namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Models;
    using uLocate.Persistance;
    using uLocate.Services;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The location type API controller for use by the umbraco back-office
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class LocationApiController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// The _request cache.
        /// </summary>
        private ICacheProvider _requestCache = ApplicationContext.Current.ApplicationCache.RequestCache;

        ///// /umbraco/backoffice/uLocate/LocationApi/Test
        ////[System.Web.Http.AcceptVerbs("GET")]
        ////public bool Test()
        ////{
        ////   return true;
        ////}

        #region Operations


        //// /umbraco/backoffice/uLocate/LocationApi/Create?LocationName=xxx

        /// <summary>
        /// Create a new Location of type "default"
        /// </summary>
        /// <param name="locationName">
        /// The location name.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/> of the newly created Location
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public Guid Create(string locationName)
        {
            var locationService = new LocationService();
            return locationService.CreateLocation(locationName);
        }

        //// /umbraco/backoffice/uLocate/LocationApi/Create?LocationName=xxx&LocationTypeGuid=xxx

        /// <summary>
        /// Create a new Location 
        /// </summary>
        /// <param name="locationName">
        /// The location name.
        /// </param>
        /// <param name="locationTypeGuid">
        /// The GUID key for the Location Type.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/> of the newly created Location
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public Guid Create(string locationName, Guid locationTypeGuid)
        {
            var locationService = new LocationService();
            return locationService.CreateLocation(locationName, locationTypeGuid);
        }

        //// /umbraco/backoffice/uLocate/LocationApi/Update

        /// <summary>
        /// Update a location 
        /// </summary>
        /// <param name="updatedLocation">
        /// The updated location object
        /// </param>
        /// <returns>
        /// The <see cref="LocationType"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public JsonLocation Update(JsonLocation updatedLocation)
        {
            var key = updatedLocation.Key;

            var entity = updatedLocation.ConvertToLocation();
            Repositories.LocationRepo.Update(entity);

            var result = Repositories.LocationRepo.GetByKey(key);
            ////uLocate.Helpers.Persistence.UpdateLocation();

            return new JsonLocation(result);
        }

        //// /umbraco/backoffice/uLocate/LocationApi/Delete

        /// <summary>
        /// Delete a location 
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public StatusMessage Delete(Guid key)
        {
            var result = Repositories.LocationRepo.Delete(key);

            return result;
        }

        #endregion

        #region Querying


        //// /umbraco/backoffice/uLocate/LocationApi/GetByKey

        /// <summary>
        /// Get a location by its Key
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="Location"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public JsonLocation GetByKey(Guid key)
        {
            var result = Repositories.LocationRepo.GetByKey(key);

            return new JsonLocation(result);
        }

        //// /umbraco/backoffice/uLocate/LocationApi/GetAll

        /// <summary>
        /// Get all locations with a matching name
        /// /umbraco/backoffice/uLocate/LocationApi/GetByName?LocName=xxx
        /// </summary>
        /// <param name="LocName">
        /// The loc name.
        /// </param>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<JsonLocation> GetByName(string LocName)
        {
            var matchingLocations = Repositories.LocationRepo.GetByName(LocName);
            var result = Repositories.LocationRepo.ConvertToJsonLocations(matchingLocations);

            return result;
        }

        /// <summary>
        /// Get all locations as a list
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<JsonLocation> GetAll()
        {
            var result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetAll());

            return result;
        }

        //// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5

        /// <summary>
        /// Get all locations as a paged list
        /// </summary>
        /// <param name="pageNum">
        /// The page number.
        /// </param>
        /// <param name="itemsPerPage">
        /// The items per page.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PageOfLocations GetAllPaged(long pageNum, long itemsPerPage)
        {
            var paged = Repositories.LocationRepo.GetPaged(pageNum + 1, itemsPerPage, string.Empty);
            var totalCount = Repositories.LocationRepo.GetCount();
            var result = new PageOfLocations() 
                            {
                                 Locations = paged,
                                 PageNum = pageNum,
                                 ItemsPerPage = itemsPerPage,
                                 TotalItems = Convert.ToInt64(totalCount)
                             };
            return result;
        }


        //// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5&OrderBy=LocationType&SearchTerm=test&SortOrder=DESC

        /// <summary>
        /// Get all locations as a paged list
        /// </summary>
        /// <param name="pageNum">
        /// Current page for results
        /// </param>
        /// <param name="itemsPerPage">
        /// Items per Page.
        /// </param>
        /// <param name="orderBy">
        /// options are: "Name" or "LocationType" (case insensitive)
        /// </param>
        /// <param name="searchTerm">
        /// The Search Term.
        /// </param>
        /// <param name="sortOrder">
        /// The sort Order.
        /// </param>
        /// <returns>
        /// An object of type <see cref="PagedLocations"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PageOfLocations GetAllPaged(int pageNum, int itemsPerPage, string orderBy, string searchTerm = "", string sortOrder = "ASC")
        {
            var locService = new LocationService();

            var allPages = (PagingCollection<JsonLocation>)_requestCache.GetCacheItem("paged-locations-search", () => locService.GetAllPages(itemsPerPage, orderBy, searchTerm, sortOrder.ToUpper()));
            
            ////var allPages = locService.GetAllPages(ItemsPerPage, OrderBy, SearchTerm);

            var result = new PageOfLocations()
            {
                Locations = allPages.GetData(pageNum),
                PageNum = pageNum,
                ItemsPerPage = itemsPerPage,
                TotalItems = allPages.TotalCount,
                TotalPages = allPages.PagesCount
            };

            return result;
        }

        //// /umbraco/backoffice/uLocate/LocationApi/GetAllPages?ItemsPerPage=5&OrderBy=LocationType&SearchTerm=test&sortOrder=DESC

        /// <summary>
        /// Get all locations as a paged list
        /// </summary>
        /// <param name="itemsPerPage">
        /// Items per Page.
        /// </param>
        /// <param name="orderBy">
        /// options are: "Name" or "LocationType" (case insensitive)
        /// </param>
        /// <param name="searchTerm">
        /// The Search Term.
        /// </param>
        /// <param name="sortOrder">
        /// The order to sort by. Can be "ASC" or "DESC". (Anything not entered as "ASC" is interpreted as "DESC")
        /// </param>
        /// <returns>
        /// An object of type PagedLocations.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PagedLocations GetAllPages(int itemsPerPage, string orderBy = "", string searchTerm = "", string sortOrder = "ASC")
        {
            var locService = new LocationService();

            var allPages = (PagingCollection<JsonLocation>)_requestCache.GetCacheItem("paged-locations-search-pages", () => locService.GetAllPages(itemsPerPage, orderBy, searchTerm, sortOrder.ToUpper()));

            var result = new PagedLocations()
            {
                ItemsPerPage = itemsPerPage,
                TotalItems = allPages.TotalCount,
                TotalPages = allPages.PagesCount
            };

            var listOfPages = new List<PageOfLocations>();
            for (var i = 0; i < allPages.PagesCount; i++)
            {
                var page = new PageOfLocations()
                               {
                                   Locations = allPages.GetData(i + 1),
                                   PageNum = i,
                                   ItemsPerPage = allPages.GetCount(i + 1),
                                   TotalItems = allPages.TotalCount,
                                   TotalPages = allPages.PagesCount
                               };

                listOfPages.Add(page);
            }

            result.Pages = listOfPages;
            return result;
        }

        //// /umbraco/backoffice/uLocate/LocationApi/GetEmptyJsonLocation

        /// <summary>
        /// Gets an empty JSON location.
        /// </summary>
        /// <returns>
        /// An empty <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public JsonLocation GetEmptyJsonLocation()
        {
            var result = new JsonLocation();
            var emptyProp = new JsonPropertyData();
            result.CustomPropertyData.Add(emptyProp);

            return result;
        }


        //// /umbraco/backoffice/uLocate/LocationApi/GetByLocationType?LocTypeKey=xxx

        /// <summary>
        /// Get all locations of a specified type
        /// </summary>
        /// <param name="locTypeKey">
        /// The Location Type Key
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<JsonLocation> GetByLocationType(Guid locTypeKey)
        {
            var filteredLocs = Repositories.LocationRepo.GetByType(locTypeKey);
            var result = Repositories.LocationRepo.ConvertToJsonLocations(filteredLocs);

            return result;
        }

        #endregion
    }
}

