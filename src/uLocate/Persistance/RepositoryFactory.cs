namespace uLocate.Persistance
{
    using System;

    using uLocate.Caching;
    using uLocate.Persistance.Repositories;

    using Umbraco.Core.Cache;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// The repository factory.
    /// </summary>
    internal class RepositoryFactory
    {
        /// <summary>
        /// The runtimeCache.
        /// </summary>
        private readonly IRuntimeCacheProvider _runtimeCache;

        /// <summary>
        /// The null cache provider.
        /// </summary>
        private readonly IRuntimeCacheProvider _nullCacheProvider = new NullCacheProvider();

        /// <summary>
        /// The Umbraco database.
        /// </summary>
        private readonly UmbracoDatabase _database;

        /// <summary>
        /// Enable caching.
        /// </summary>
        private bool _enableCaching = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFactory"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="runtimeCache">
        /// The runtimeCache.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws exception if either database or runtimeCache is null
        /// </exception>
        public RepositoryFactory(UmbracoDatabase database, IRuntimeCacheProvider runtimeCache)
        {
            if (database == null) throw new ArgumentNullException("database");
            if (runtimeCache == null) throw new ArgumentNullException("runtimeCache");

            this._runtimeCache = runtimeCache;

            _database = database;
        }

        /// <summary>
        /// Gets or sets a value indicating whether caching is enabled.
        /// </summary>
        public bool EnableCaching
        {
            get
            {
                return _enableCaching;
            }

            set
            {
                _enableCaching = value;
            }
        }

        /// <summary>
        /// Creates an instance of the <see cref="ILocationTypeDefinitionRepository"/>
        /// </summary>
        /// <returns>
        /// The <see cref="ILocationTypeDefinitionRepository"/>.
        /// </returns>
        public ILocationTypeDefinitionRepository CreateLocationTypeDefinitionRepository()
        {
            return new LocationTypeDefinitionRepository(_database, _enableCaching ? _runtimeCache : _nullCacheProvider);
        }
    }
}