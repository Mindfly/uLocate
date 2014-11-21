using Umbraco.Core.Models.EntityBase;

namespace uLocate.Persistance.Repositories
{
    using Models;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// The paged repository base.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of TEntity
    /// </typeparam>
    internal abstract class PagedRepositoryBase<TEntity> : RepositoryBase<TEntity>
        where TEntity : IEntity

    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedRepositoryBase{TEntity}"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        protected PagedRepositoryBase(UmbracoDatabase database, IRuntimeCacheProvider cache) 
            : base(database, cache)
        {
        }

        /// <summary>
        /// Gets a <see cref="Page{T}"/>
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="itemsPerPage">
        /// The items per page.
        /// </param>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <returns>
        /// The <see cref="Page{TEntity}"/>.
        /// </returns>
        public abstract Page<TEntity> Page(long page, long itemsPerPage, Sql sql);  
    }
}