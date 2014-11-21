namespace uLocate.Caching
{
    using System;

    using uLocate.Models;

    /// <summary>
    /// A utility class to assist in building and maintaining unique cache keys
    /// </summary>
    internal class CacheKeys
    {
        /// <summary>
        /// The get geocode request cache key.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="formattedAddress">
        /// The formatted address
        /// </param>
        /// <returns>
        /// The cache key <see cref="string"/>.
        /// </returns>
        public static string GetGeocodeRequestCacheKey(Type provider, string formattedAddress)
        {
            return string.Format("ulocate.{0}.{1}", provider.Name, formattedAddress);
        }

        /// <summary>
        /// Gets a cache key used by repositories for caching entities
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of <see cref="IEntity"/>
        /// </typeparam>
        /// <returns>
        /// The cache key <see cref="string"/>.
        /// </returns>
        public static string GetEntityCacheKey<TEntity>(Guid key) where TEntity : IEntity
        {
            return string.Format("ulocate.{0}.{1}", typeof(TEntity), key);
        }
    }
}