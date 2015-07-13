namespace uLocate.UI.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Models;
    using uLocate.Services;

    using Umbraco.Core.Logging;
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
        //private ICacheProvider _requestCache = ApplicationContext.Current.ApplicationCache.RequestCache;

        private LocationService locationService = new LocationService();
        private LocationTypeService locationTypeService = new LocationTypeService();

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
        public IndexedLocation Update(IndexedLocation updatedLocation)
        {
            //OLD
            //var key = updatedLocation.Key;
            //var entity = updatedLocation.ConvertToLocation();
            //Repositories.LocationRepo.Update(entity);
            //var result = Repositories.LocationRepo.GetByKey(key);

            var result = locationService.UpdateLocation(updatedLocation);

            return result;
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
            var result = locationService.DeleteLocation(key);

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
        /// The <see cref="EditableLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IndexedLocation GetByKey(Guid key)
        {
            var result = locationService.GetLocation(key);

            return result;
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
        public IEnumerable<IndexedLocation> GetByName(string LocName)
        {
            var result = locationService.GetLocations(LocName);

            return result;
        }

        /// <summary>
        /// Get all locations as a list
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<IndexedLocation> GetAll()
        {
            //OLD
            //var result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetAll());

            var result = locationService.GetLocations();

            return result;
        }


        //// /umbraco/backoffice/uLocate/LocationApi/GetByLocationType?LocTypeKey=xxx

        /// <summary>
        /// Get all locations of a specified type
        /// </summary>
        /// <param name="locTypeKey">
        /// The Location Type Key
        /// </param>
        /// <param name="LogInfo">
        /// Log info about this request?
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<IndexedLocation> GetByLocationType(Guid locTypeKey, bool LogInfo = false)
        {
            var result = this.locationService.GetLocations(locTypeKey);

            if (LogInfo)
            {
                var msg = string.Format("GetByLocationType [{0}]: Total={1}", locTypeKey.ToString(), result.Count());
                LogHelper.Info<LocationApiController>(msg);
            }

            return result;
        }

        //// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5

        /// <summary>
        /// Get all locations as a paged list
        /// </summary>
        /// <param name="pageNum">
        /// The page number. (0-based index assumed)
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
            var allPages = locationService.GetAllPages(Convert.ToInt32(itemsPerPage),"", "");

            var result = new PageOfLocations()
            {
                Locations = allPages.GetData(Convert.ToInt32(pageNum + 1)),
                PageNum = pageNum,
                ItemsPerPage = itemsPerPage,
                TotalItems = allPages.TotalCount,
                TotalPages = allPages.PagesCount
            };
            return result;
        }


        //// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5&OrderBy=LocationType&SearchTerm=test&SortOrder=DESC

        /// <summary>
        /// Get all locations as a paged list
        /// </summary>
        /// <param name="pageNum">
        /// Current page for results (0-based index assumed)
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
        /// An object of type <see cref="PageOfLocations"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PageOfLocations GetAllPaged(int pageNum, int itemsPerPage, string orderBy, string searchTerm = "", string sortOrder = "ASC")
        {
            //var allPages = (PagingCollection<IndexedLocation>)_requestCache.GetCacheItem("paged-locations-search", () => locationService.GetAllPages(itemsPerPage, orderBy, searchTerm, sortOrder.ToUpper()));

            var allPages = locationService.GetAllPages(itemsPerPage, orderBy, searchTerm, sortOrder.ToUpper());

            var result = new PageOfLocations()
            {
                Locations = allPages.GetData(Convert.ToInt32(pageNum + 1)),
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
            //var allPages = (PagingCollection<IndexedLocation>)_requestCache.GetCacheItem("paged-locations-search-pages", () => locationService.GetAllPages(itemsPerPage, orderBy, searchTerm, sortOrder.ToUpper()));

            var allPages = locationService.GetAllPages(itemsPerPage, orderBy, searchTerm, sortOrder.ToUpper());

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
        /// An empty <see cref="IndexedLocation"/>.
        /// </returns>
        //[System.Web.Http.AcceptVerbs("GET")]
        //public IndexedLocation GetEmptyJsonLocation()
        //{
        //    var result = new IndexedLocation();
        //    var emptyProp = new IndexedPropertyData();
        //    result.CustomPropertyData.Add(emptyProp);

        //    return result;
        //}




        #endregion
    }
}

