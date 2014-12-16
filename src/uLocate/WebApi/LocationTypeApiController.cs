namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using uLocate.Models;
    using uLocate.Persistance;
    using uLocate.Data;
    using Umbraco.Core;
    using Umbraco.Web.WebApi;

    //TODO: Add to back-office App area / secure this

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

        [AcceptVerbs("GET")]
        public LocationType Update(LocationType UpdatedLocationType)
        {
            Repositories.LocationTypeRepo.Update(UpdatedLocationType);

            var Result = Repositories.LocationTypeRepo.GetByKey(UpdatedLocationType.Key);

            return Result;
        }

        [AcceptVerbs("GET")]
        public LocationType GetByKey(Guid Key)
        {
            var Result = Repositories.LocationTypeRepo.GetByKey(Key);

            return Result;
        }

        [AcceptVerbs("GET")]
        public StatusMessage Delete(Guid Key)
        {
            var Result = Repositories.LocationTypeRepo.Delete(Key, true);

            return Result;
        }


    }
}

