using uLocate.Models.Rdbms;

namespace uLocate.Persistance.Repositories
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Represents a location repository.
    /// </summary>
    internal class LocationRepository : PagedRepositoryBase<ILocation>, ILocationRespository
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


        protected override IEnumerable<ILocation> PerformGetAll(params Guid[] keys)
        {
            throw new NotImplementedException();
        }

        protected override ILocation PerformGet(Guid key)
        {
            throw new NotImplementedException();
        }

        protected override void PersistNewItem(ILocation item)
        {
            throw new NotImplementedException();
        }

        protected override void PersistUpdatedItem(ILocation item)
        {
            throw new NotImplementedException();
        }

        protected override void PersistDeletedItem(ILocation item)
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
                .InnerJoin<LocationTypeDefinitionDto>()
                .On<LocationDto, LocationTypeDefinitionDto>(left => left.LocationTypeKey, right => right.Key);

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
                "DELETE FROM ulocateLocation WHERE pk = @Key";
            };

            return list;
        }

        public override Page<ILocation> Page(long page, long itemsPerPage, Sql sql)
        {
            throw new NotImplementedException();
        }
    }
}