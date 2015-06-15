namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using uLocate.Models;
    using uLocate.Persistance;

    using umbraco.cms.businesslogic.packager;

    using Umbraco.Core.Media;
    using Umbraco.Web.WebApi;

    public class LocationSearchApiController : UmbracoApiController
    {
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
            var locatonPropertyData = Repositories.LocationPropertyDataRepo.GetAll().ToList();
            var propertyData = new List<KeyValuePair<string, string>>();
            foreach (var prop in locatonPropertyData)
            {
                if (prop.PropertyAlias == Alias)
                {
                    propertyData.Add(new KeyValuePair<string, string>(prop.Value.ToString(), prop.LocationKey.ToString()));
                }
            }
            return propertyData;
        }

        [System.Web.Http.AcceptVerbs("GET")]
        public JsonLocation GetByKey(Guid Key)
        {
            var Result = Repositories.LocationRepo.GetByKey(Key);

            return new JsonLocation(Result);
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
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> Search(double Lat, double Long, int Miles)
        {
            var Result =
                Repositories.LocationRepo.ConvertToJsonLocations(
                    Repositories.LocationRepo.GetByGeoSearch(Lat, Long, Miles));

            return Result;
        }

        // /umbraco/api/LocationSearchApi/Search?Lat=40.7762965&Long=-73.950718&Miles=20&LocType=xxx
        /// <summary>
        /// Search for locations within a certain distance from a provided lat/long
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Miles">Maximum distance away in miles</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> Search(double Lat, double Long, int Miles, Guid LocType)
        {
            var Result =
                Repositories.LocationRepo.ConvertToJsonLocations(
                    Repositories.LocationRepo.GetByGeoSearch(Lat, Long, Miles, LocType));

            return Result;
        }

        // /umbraco/api/LocationSearchApi/Search?Lat=40.7762965&Long=-73.950718&Miles=20&LocTypeAlias=Business
        /// <summary>
        /// Search for locations within a certain distance from a provided lat/long
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Miles">Maximum distance away in miles</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> Search(double Lat, double Long, int Miles, string LocTypeAlias)
        {
            var LocType = Repositories.LocationTypeRepo.GetByName(LocTypeAlias).FirstOrDefault().Key;
            var Result =
                Repositories.LocationRepo.ConvertToJsonLocations(
                    Repositories.LocationRepo.GetByGeoSearch(Lat, Long, Miles, LocType));

            return Result;
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
        public IEnumerable<JsonLocation> Search(string postalCode)
        {
            var result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetByPostalCode(postalCode));

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
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> GetNearestLocations(double Lat, double Long, int Qty)
        {
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetNearestLocations(Lat, Long, Qty));

            return Result;
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
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> GetNearestLocations(double Lat, double Long, int Qty, Guid LocType)
        {
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetNearestLocations(Lat, Long, Qty, LocType));

            return Result;
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
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> GetNearestLocations(double Lat, double Long, int Qty, string LocTypeAlias)
        {
            var LocType = Repositories.LocationTypeRepo.GetByName(LocTypeAlias).FirstOrDefault().Key;
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetNearestLocations(Lat, Long, Qty, LocType));

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
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> GetByCountry(string CountryCode)
        {
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetByCountry(CountryCode));

            return Result;
        }

        // /umbraco/api/LocationSearchApi/GetNearestLocations?CountryCode=GB&LocType=xxx
        /// <summary>
        /// Get the locations with a matching Country Code.
        /// </summary>
        /// <param name="CountryCode">A 2-character Country Code</param>
        /// <param name="LocType">Location Type Key to filter by</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> GetByCountry(string CountryCode, Guid LocType)
        {
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetByCountry(CountryCode, LocType));

            return Result;
        }

        // /umbraco/api/LocationSearchApi/GetNearestLocations?CountryCode=GB&LocTypeAlias=Business
        /// <summary>
        /// Get the locations with a matching Country Code.
        /// </summary>
        /// <param name="CountryCode">A 2-character Country Code</param>
        /// <param name="LocTypeAlias">Location Type Alias to filter by</param>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="JsonLocation"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> GetByCountry(string CountryCode, string LocTypeAlias)
        {
            var LocType = Repositories.LocationTypeRepo.GetByName(LocTypeAlias).FirstOrDefault().Key;
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetByCountry(CountryCode, LocType));

            return Result;
        }
        #endregion
    }
}
