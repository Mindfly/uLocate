namespace uLocate.UI.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using uLocate.Models;
    using uLocate.Services;

    using Umbraco.Web.WebApi;

    public class LocationSearchApiController : UmbracoApiController
    {
        private LocationService locationService = new LocationService();
        private LocationTypeService locationTypeService = new LocationTypeService();

        /// <summary>
        /// Used for testing
        /// /umbraco/api/LocationSearchApi/Test
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public bool Test()
        {
            return true;
        }

        [AcceptVerbs("GET", "POST")]
        public IEnumerable<KeyValuePair<string, string>> GetAllPropertyDataByAlias(string Alias)
        {
            //TODO: Check if we need this at all

            var locationPropertyData = this.locationService.GetAllPropertyData();
            var propertyData = new List<KeyValuePair<string, string>>();
            foreach (var prop in locationPropertyData)
            {
                if (prop.PropertyAlias == Alias)
                {
                    propertyData.Add(new KeyValuePair<string, string>(prop.Value.ToString(), prop.LocationKey.ToString()));
                }
            }
            return propertyData;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public IndexedLocation GetByKey(Guid Key)
        {
            var result = this.locationService.GetLocation(Key);

            return result;
        }

        #region Search by Miles

        // /umbraco/api/LocationSearchApi/Search?Lat=40.7762965&Long=-73.950718&Miles=20
        /// <summary>
        /// Search for locations within a certain distance from a provided lat/long
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Miles">Maximum distance away in miles</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> Search(double Lat, double Long, int Miles)
        {
            var result =this.locationService.GetByGeoSearch(Lat, Long, Miles);

            return result;
        }

        // /umbraco/api/LocationSearchApi/Search?Lat=40.7762965&Long=-73.950718&Miles=20&LocType=xxx
        /// <summary>
        /// Search for locations within a certain distance from a provided lat/long
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Miles">Maximum distance away in miles</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> Search(double Lat, double Long, int Miles, Guid LocType)
        {
            var result = this.locationService.GetByGeoSearch(Lat, Long, Miles, LocType);
               
            return result;
        }

        // /umbraco/api/LocationSearchApi/Search?Lat=40.7762965&Long=-73.950718&Miles=20&LocTypeAlias=Business
        /// <summary>
        /// Search for locations within a certain distance from a provided lat/long
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Miles">Maximum distance away in miles</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> Search(double Lat, double Long, int Miles, string LocTypeAlias)
        {
            var locTypeKey = this.locationTypeService.GetLocationType(LocTypeAlias).Key;
            var result = this.locationService.GetByGeoSearch(Lat, Long, Miles, locTypeKey); 

            return result;
        }

        /// <summary>
        /// The search.
        /// </summary>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> Search(string postalCode)
        {
            var result = this.locationService.GetLocationsByPostalCode(postalCode);

            return result;            
        }

        #endregion

        #region Get Nearest Qty
        // /umbraco/api/LocationSearchApi/GetNearestLocations?Lat=40.7762965&Long=-73.950718&Qty=10
        /// <summary>
        /// Get the X locations nearest to a Lat/Long point.
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Qty"> Quantity of locations to return</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> GetNearestLocations(double Lat, double Long, int Qty)
        {
            var result = this.locationService.GetNearestLocations(Lat, Long, Qty);

            return result;
        }

        // /umbraco/api/LocationSearchApi/GetNearestLocations?Lat=40.7762965&Long=-73.950718&Qty=10&LocType=xxx
        /// <summary>
        /// Get the X locations nearest to a Lat/Long point.
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Qty">Quantity of locations to return</param>
        /// <param name="LocType">Location Type Key to filter by</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> GetNearestLocations(double Lat, double Long, int Qty, Guid LocType)
        {
            var result = this.locationService.GetNearestLocations(Lat, Long, Qty, LocType);

            return result;
        }

        // /umbraco/api/LocationSearchApi/GetNearestLocations?Lat=40.7762965&Long=-73.950718&Qty=10&LocTypeAlias=Business
        /// <summary>
        /// Get the X locations nearest to a Lat/Long point.
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Qty">Quantity of locations to return</param>
        /// <param name="LocTypeAlias">Location Type Alias to filter by</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> GetNearestLocations(double Lat, double Long, int Qty, string LocTypeAlias)
        {
            var locTypeKey = locationTypeService.GetLocationType(LocTypeAlias).Key;

            var Result = locationService.GetNearestLocations(Lat, Long, Qty, locTypeKey);
              
            return Result;
        } 


        #endregion

        #region Get by Country Code

        // /umbraco/api/LocationSearchApi/GetByCountry?CountryCode=GB

        /// <summary>
        /// Get the locations with a matching Country Code.
        /// </summary>
        /// <param name="CountryCode">A 2-character Country Code</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> GetByCountry(string CountryCode)
        {
            var Result = locationService.GetLocationsByCountry(CountryCode);

            return Result;
        }

        // /umbraco/api/LocationSearchApi/GetNearestLocations?CountryCode=GB&LocType=xxx

        /// <summary>
        /// Get the locations with a matching Country Code.
        /// </summary>
        /// <param name="CountryCode">A 2-character Country Code</param>
        /// <param name="LocType">Location Type Key to filter by</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> GetByCountry(string CountryCode, Guid LocType)
        {
            var result = locationService.GetLocationsByCountry(CountryCode, LocType);

            return result;
        }

        // /umbraco/api/LocationSearchApi/GetNearestLocations?CountryCode=GB&LocTypeAlias=Business
        /// <summary>
        /// Get the locations with a matching Country Code.
        /// </summary>
        /// <param name="CountryCode">A 2-character Country Code</param>
        /// <param name="LocTypeAlias">Location Type Alias to filter by</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="IndexedLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<IndexedLocation> GetByCountry(string CountryCode, string LocTypeAlias)
        {
            var locTypeKey = locationTypeService.GetLocationType(LocTypeAlias).Key;
            var result = locationService.GetLocationsByCountry(CountryCode, locTypeKey);

            return result;
        }
        #endregion
    }
}
