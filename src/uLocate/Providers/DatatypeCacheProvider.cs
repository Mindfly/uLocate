namespace uLocate.Providers
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// The data type cache provider.
    /// </summary>
    public class DataTypeCacheProvider
    {
        /// <summary>
        /// The cache key.
        /// </summary>
        private const string CacheKey = "umbukfest_all_datatypes";

        /// <summary>
        /// The current cache provider
        /// </summary>
        private static DataTypeCacheProvider current;

        /// <summary>
        /// Gets the current cache provider
        /// </summary>
        public static DataTypeCacheProvider Current
        {
            get
            {
                return current ?? (current = new DataTypeCacheProvider());
            }
        }

        /// <summary>
        /// Sets the object in  the cache
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void Set(string key, object value)
        {
            MemoryCache.Default.Set(key, value, DateTimeOffset.UtcNow.AddYears(1));
        }

        /// <summary>
        /// Gets the object from cache
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Get(string key)
        {
            return MemoryCache.Default.Get(key);
        }

        /// <summary>
        /// Clears the cache
        /// </summary>
        public void Clear()
        {
            MemoryCache.Default.Remove(CacheKey);
        }

        /// <summary>
        /// Gets or sets a object returned by the action in cache
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>        
        /// <typeparam name="TOutput">
        ///  The type of object
        /// </typeparam>
        /// <returns>
        /// The <see cref="TOutput"/>.
        /// </returns>
        public TOutput GetOrExecute<TOutput>(Func<TOutput> action)
        {
            object cachedObject = this.Get(CacheKey);

            if (cachedObject == null)
            {
                cachedObject = action();

                if (cachedObject != null)
                {
                    this.Set(CacheKey, cachedObject);
                }
            }

            return (TOutput)cachedObject;
        }
    }
}
