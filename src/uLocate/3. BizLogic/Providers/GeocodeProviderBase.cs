namespace uLocate.Providers
{
    using System;
    using System.Collections.Generic;
    using Models;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;

    /// <summary>
    /// The geocode provider base.
    /// </summary>
    public abstract class GeocodeProviderBase : IGeocodeProvider
    {
        /// <summary>
        /// The <see cref="IRuntimeCacheProvider"/>
        /// </summary>
        private readonly IRuntimeCacheProvider _cache;

        /// <summary>
        /// The geocode provider settings.
        /// </summary>
        private readonly GeocodeProviderSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodeProviderBase"/> class.
        /// </summary>
        /// <param name="cache">
        /// The cache.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws an <see cref="ArgumentNullException"/> if the cache parameter is omitted
        /// </exception>
        protected GeocodeProviderBase(IRuntimeCacheProvider cache)            
        {
            if (cache == null) throw new ArgumentNullException("cache");

            _cache = cache;

            _settings = new GeocodeProviderSettings(GetType());
        }


        /// <summary>
        /// Gets the settings.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Settings
        {
            get { return _settings.Settings; }            
        }

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        public virtual IGeocodeProviderResponse Geocode(IAddress address)
        {
            return TryGetGeocode(address.AsApiRequestFormattedAddressString());
        }

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        /// <param name="countryCode">
        /// The country code.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        public virtual IGeocodeProviderResponse Geocode(string postalCode, string countryCode)
        {
            return Geocode(string.Empty, string.Empty, postalCode, countryCode);
        }

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="locality">
        /// The locality.
        /// </param>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        /// <param name="countryCode">
        /// The country code.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        public virtual IGeocodeProviderResponse Geocode(string locality, string region, string postalCode, string countryCode)
        {
            return Geocode(string.Empty, string.Empty, locality, region, postalCode, countryCode);
        }

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="address1">
        /// The address 1.
        /// </param>
        /// <param name="address2">
        /// The address 2.
        /// </param>
        /// <param name="locality">
        /// The locality.
        /// </param>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        /// <param name="countryCode">
        /// The country code.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        public virtual IGeocodeProviderResponse Geocode(string address1, string address2, string locality, string region, string postalCode, string countryCode)
        {
            return TryGetGeocode(UtilityExtensions.GetApiRequestFormattedAddressString(address1, address2, locality, region, postalCode, countryCode));        
        }

        /// <summary>
        /// Gets the geocode response from the API
        /// </summary>
        /// <param name="formattedAddress">
        /// The formatted address.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        protected abstract IGeocodeProviderResponse GetGeocodeProviderResponse(string formattedAddress);

        /// <summary>
        /// The perform geocode.
        /// </summary>
        /// <param name="formattedAddress">
        /// The formatted address.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>        
        private IGeocodeProviderResponse TryGetGeocode(string formattedAddress)
        {
            return _settings.EnableCaching
                ? (IGeocodeProviderResponse)_cache
                    .GetCacheItem(
                        Caching.CacheKeys.GetGeocodeRequestCacheKey(GetType(), formattedAddress),
                        () => GetResponse(formattedAddress),
                        new TimeSpan(0, 0, 0, _settings.CacheDuration))
                : GetResponse(formattedAddress);            
        }

        /// <summary>
        /// The get response.
        /// </summary>
        /// <param name="formattedAddress">
        /// The formatted address.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        private IGeocodeProviderResponse GetResponse(string formattedAddress)
        {
            var response = GetGeocodeProviderResponse(formattedAddress);

            if (_settings.LogRequests)
            LogHelper.Info<GeocodeProviderBase>(string.Format("Attempt to geocode {0} status returned: {1}", formattedAddress, response.Status.ToString()));

            return response;
        }
    }
}