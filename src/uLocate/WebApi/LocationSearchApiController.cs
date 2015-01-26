namespace uLocate.WebApi
{
    using System.Collections.Generic;
    using System.Web.Http;

    using uLocate.Models;
    using uLocate.Persistance;

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

        /// <summary>
        /// Search for locations within a certain distance from a provided lat/long
        /// /umbraco/api/LocationSearchApi/Search?Lat=40.7762965&Long=-73.950718&Miles=20
        /// </summary>
        /// <param name="Lat">Latitude of search point</param>
        /// <param name="Long">Longitude of search point</param>
        /// <param name="Miles">Maximum distance away in miles</param>
        /// <returns></returns>
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public IEnumerable<JsonLocation> Search(double Lat, double Long, int Miles)
        {
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetByGeoSearch(Lat,  Long,  Miles));

            return Result;
        }

    }
}
