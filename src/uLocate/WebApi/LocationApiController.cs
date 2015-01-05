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
        /// Create a new Location 
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
            Location newLoc = new Location();
            newLoc.Name = LocationName;
            Repositories.LocationRepo.Insert(newLoc);

            //var Result = Repositories.LocationTypeRepo.GetByKey(newLocType.Key);
            var Result = newLoc.Key;
            
            return Result;
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
        [System.Web.Http.AcceptVerbs("GET")]
        public Location Update(Location UpdatedLocation)
        {
            Repositories.LocationRepo.Update(UpdatedLocation);

            var Result = Repositories.LocationRepo.GetByKey(UpdatedLocation.Key);

            return Result;
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
        public Location GetByKey(Guid Key)
        {
            var Result = Repositories.LocationRepo.GetByKey(Key);

            return Result;
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
        public List<Location> GetAll()
        {
            var Result = Repositories.LocationRepo.GetAll().ToList();

            return Result;
        }

        /// <summary>
        /// Get all locations as a paged list
        /// /umbraco/backoffice/uLocate/LocationApi/GetAllPaged?PageNum=1&ItemsPerPage=5
        /// </summary>
        /// <param name="PageNum">
        /// The page num.
        /// </param>
        /// <param name="ItemsPerPage">
        /// The items per page.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public List<Location> GetAllPaged(long PageNum, long ItemsPerPage)
        {
            var Result = Repositories.LocationRepo.GetPaged(PageNum, ItemsPerPage, string.Empty);

            return Result;
        }

    }
}

