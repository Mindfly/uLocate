using uLocate.Data;

namespace uLocate.Persistance
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Represents a location repository.
    /// </summary>
    internal class LocationRepository : PagedRepositoryBase<Location> //, ILocationRespository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepository"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public LocationRepository(UmbracoDatabase database, IRuntimeCacheProvider cache)
            : base(database, cache)
        {            
        }

        public override Page<Location> Page(long page, long itemsPerPage, Sql sql)
        {
            throw new NotImplementedException();
        }

        public void Insert(Location LocationEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Location LocationEntity)
        {
            throw new NotImplementedException();
        }

        public Location GetByKey(Guid Key)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Location> PerformGetAll(params object[] IdKeys)
        {
            throw new NotImplementedException();
        }

        protected override Location PerformGet(object IdKey)
        {
            throw new NotImplementedException();
        }

        protected override object PersistNewItem(Location item)
        {
            throw new NotImplementedException();
        }

        protected override void PersistUpdatedItem(Location item)
        {
            throw new NotImplementedException();
        }

        protected override void PersistDeletedItem(Location item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the base sql string
        /// </summary>
        /// <param name="isCount">
        /// The is count.
        /// </param>
        /// <returns>
        /// The <see cref="Sql"/>.
        /// </returns>
        protected override Sql GetBaseQuery(bool isCount)
        {
            var sql = new Sql();
            sql.Select(isCount ? "COUNT(*)" : "*")
                .From<LocationDto>()
                .InnerJoin<LocationType>()
                .On<LocationDto, LocationType>(left => left.LocationTypeId, right => right.Id);

            return sql;
        }

        /// <summary>
        /// Gets the base where clause
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected override string GetBaseWhereClause()
        {
            return "ulocateLocation.pk = @Key";
        }

        protected override IEnumerable<string> GetDeleteClauses()
        {
            var list = new List<string>
            {
                "DELETE FROM ulocateLocation WHERE pk = @Key"
            };

            return list;
        }


    }
}