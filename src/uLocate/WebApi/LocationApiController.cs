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
    using uLocate.Services;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The location type api controller for use by the umbraco back-office
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class LocationApiController : UmbracoAuthorizedApiController
    {
        private ICacheProvider _requestCache = ApplicationContext.Current.ApplicationCache.RequestCache;

        ///// /umbraco/backoffice/uLocate/LocationApi/Test
        //[System.Web.Http.AcceptVerbs("GET")]
        //public bool Test()
        //{
        //    return true;
        //}

        #region Operations

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
            var locationService = new LocationService();
            return locationService.CreateLocation(LocationName);
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
            var locationService = new LocationService();
            return locationService.CreateLocation(LocationName, LocationTypeGuid);
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
            var key = UpdatedLocation.Key;

            var entity = UpdatedLocation.ConvertToLocation();
            Repositories.LocationRepo.Update(entity);

            var Result = Repositories.LocationRepo.GetByKey(key);
            //uLocate.Helpers.Persistence.UpdateLocation();

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

        #endregion

        #region Querying

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
        public PageOfLocations GetAllPaged(long PageNum, long ItemsPerPage)
        {
            var paged = Repositories.LocationRepo.GetPaged(PageNum + 1, ItemsPerPage, string.Empty);
            var totalCount = Repositories.LocationRepo.GetCount();
            var result = new PageOfLocations() 
                            {
                                 Locations = paged,
                                 PageNum = PageNum,
                                 ItemsPerPage = ItemsPerPage,
                                 TotalItems = Convert.ToInt64(totalCount)
                             };

            return result;
        }

        //umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5&OrderBy=LocationType

        /// <summary>
        /// Get all locations as a paged list
        /// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&amp;ItemsPerPage=5&amp;OrderBy=LocationType
        /// </summary>
        /// <param name="PageNum">
        /// Current page for results
        /// </param>
        /// <param name="ItemsPerPage">
        /// Items per Page.
        /// </param>
        /// <param name="OrderBy">
        /// options are: "Name" or "LocationType" (case insensitive)
        /// </param>
        /// <returns>
        /// An object of type <see cref="PagedLocations"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PageOfLocations GetAllPaged(int PageNum, int ItemsPerPage, string OrderBy)
        {
            var locService = new LocationService();

            var allPages = (PagingCollection<JsonLocation>)_requestCache.GetCacheItem("paged-locations", () => locService.GetAllPages(ItemsPerPage, OrderBy, string.Empty));

            //var allPages = locService.GetAllPages(ItemsPerPage, OrderBy, string.Empty);
           // var allPages = Repositories.LocationRepo.GetPaged(ItemsPerPage, OrderBy, string.Empty);

            var result = new PageOfLocations()
            {
                Locations = allPages.GetData(PageNum),
                PageNum = PageNum,
                ItemsPerPage = ItemsPerPage,
                TotalItems = allPages.TotalCount
            };

            return result;
        }

        //umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5&OrderBy=LocationType&SearchTerm=test

        /// <summary>
        /// Get all locations as a paged list
        /// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&amp;ItemsPerPage=5&amp;OrderBy=LocationType&amp;SearchTerm=test
        /// </summary>
        /// <param name="PageNum">
        /// Current page for results
        /// </param>
        /// <param name="ItemsPerPage">
        /// Items per Page.
        /// </param>
        /// <param name="OrderBy">
        /// options are: "Name" or "LocationType" (case insensitive)
        /// </param>
        /// <param name="SearchTerm">
        /// The Search Term.
        /// </param>
        /// <returns>
        /// An object of type <see cref="PagedLocations"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PageOfLocations GetAllPaged(int PageNum, int ItemsPerPage, string OrderBy, string SearchTerm)
        {
            var locService = new LocationService();

            var allPages = (PagingCollection<JsonLocation>)_requestCache.GetCacheItem("paged-locations-search", () => locService.GetAllPages(ItemsPerPage, OrderBy, SearchTerm));
            
            //var allPages = locService.GetAllPages(ItemsPerPage, OrderBy, SearchTerm);

            var result = new PageOfLocations()
            {
                Locations = allPages.GetData(PageNum),
                PageNum = PageNum,
                ItemsPerPage = ItemsPerPage,
                TotalItems = allPages.TotalCount,
                TotalPages = allPages.PagesCount
            };

            return result;
        }

        //umbraco/backoffice/uLocate/LocationApi/GetAllPages?ItemsPerPage=5&OrderBy=LocationType&SearchTerm=test

        /// <summary>
        /// Get all locations as a paged list
        /// /umbraco/backoffice/uLocate/LocationApi/GetAllPages?ItemsPerPage=5&amp;OrderBy=LocationType&amp;SearchTerm=test
        /// </summary>
        /// <param name="ItemsPerPage">
        /// Items per Page.
        /// </param>
        /// <param name="OrderBy">
        /// options are: "Name" or "LocationType" (case insensitive)
        /// </param>
        /// <param name="SearchTerm">
        /// The Search Term.
        /// </param>
        /// <returns>
        /// An object of type <see cref="PagingCollection<JsonLocation>"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public PagedLocations GetAllPages(int ItemsPerPage, string OrderBy, string SearchTerm)
        {
            var locService = new LocationService();

            var allPages = (PagingCollection<JsonLocation>)_requestCache.GetCacheItem("paged-locations-search-pages", () => locService.GetAllPages(ItemsPerPage, OrderBy, SearchTerm));

            var result = new PagedLocations()
            {
                ItemsPerPage = ItemsPerPage,
                TotalItems = allPages.TotalCount,
                TotalPages = allPages.PagesCount
            };

            var listOfPages = new List<PageOfLocations>();
            for (int i = 0; i < allPages.PagesCount; i++)
            {
                var page = new PageOfLocations()
                               {
                                   Locations = allPages.GetData(i+1),
                                   PageNum = i+1,
                                   ItemsPerPage = allPages.GetCount(i+1),
                                   TotalItems = allPages.TotalCount,
                                   TotalPages = allPages.PagesCount
                               };

                listOfPages.Add(page);
            }
            result.Pages = listOfPages;

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
        /// Get all locations of a specified type
        /// /umbraco/backoffice/uLocate/LocationApi/GetByLocationType?LocTypeKey=xxx
        /// </summary>
        /// <param name="LocTypeKey">
        /// The Location Type Key
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<JsonLocation> GetByLocationType(Guid LocTypeKey)
        {
            var FilteredLocs = Repositories.LocationRepo.GetByType(LocTypeKey);
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(FilteredLocs);

            return Result;
        }

        #endregion

    }
}

