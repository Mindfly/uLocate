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

    //TODO: Add to back-office App area / secure this

    /// <summary>
    /// The location type api controller for use by the umbraco back-office
    /// </summary>
    class LocationTypeApiController : UmbracoApiController
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="LocationTypeName">
        /// The location type name.
        /// </param>
        /// <returns>
        /// The <see cref="Guid"/> of the newly created LocationType
        /// </returns>
        [AcceptVerbs("GET")]
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
        /// </summary>
        /// <param name="UpdatedLocationType">
        /// The updated location type.
        /// </param>
        /// <returns>
        /// The <see cref="LocationType"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public LocationType Update(LocationType UpdatedLocationType)
        {
            Repositories.LocationTypeRepo.Update(UpdatedLocationType);

            var Result = Repositories.LocationTypeRepo.GetByKey(UpdatedLocationType.Key);

            return Result;
        }

        /// <summary>
        /// Get a location type by its Key
        /// </summary>
        /// <param name="Key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="LocationType"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public LocationType GetByKey(Guid Key)
        {
            var Result = Repositories.LocationTypeRepo.GetByKey(Key);

            return Result;
        }

        /// <summary>
        /// Delete a location type
        /// </summary>
        /// <param name="Key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public StatusMessage Delete(Guid Key)
        {
            var Result = Repositories.LocationTypeRepo.Delete(Key, true);

            return Result;
        }


    }
}

