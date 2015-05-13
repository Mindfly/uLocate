using Umbraco.Web.PropertyEditors;

namespace uLocate.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Configuration;
    using Umbraco.Core;

    /// <summary>
    /// An abstract class that should be used for all GeocodingProviders
    /// </summary>
    public class GeocodeProviderSettings
    {
        /// <summary>
        /// The uLocate configuration section.
        /// </summary>
        private static readonly uLocateSection Section = (uLocateSection)ConfigurationManager.GetSection("uLocate");

        /// <summary>
        /// The provider configuration element in the uLocate configuration section
        /// </summary>
        private readonly ProviderElement _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodeProviderSettings"/> class.
        /// </summary>
        /// <param name="providerType">
        /// The type of the provider - used to retrieve the attribute information
        /// </param>
        /// <exception cref="ArgumentException">
        /// Throws an exception if alias is not provided
        /// </exception>
        public GeocodeProviderSettings(Type providerType)
        {
            var att = providerType.GetCustomAttribute<GeocodeProviderAttribute>(true);

            if (att == null) throw new ArgumentException("The type provided did not have a valid GeocodeProviderAttribute");

            _configuration = Section.Providers[att.Alias];
        }

        /// <summary>
        /// Gets a value indicating whether the Geocode Requests should be logged.
        /// </summary>
        public bool LogRequests
        {
            get { return _configuration.LogRequests; }            
        }

        /// <summary>
        /// Gets a value indicating whether or not to enable caching.
        /// </summary>
        public bool EnableCaching
        {
            get { return _configuration.EnableCaching; }
        }

        /// <summary>
        /// Gets the cache duration in seconds
        /// </summary>
        public int CacheDuration
        {
            get { return _configuration.CacheDuration; }
        }

        /// <summary>
        /// Gets the geocode limit value.
        /// </summary>
        /// <remarks>
        /// Some providers restrict the number of geocoding requests can be performed
        /// </remarks>
        public int GeocodeRequestLimit
        {
            get { return _configuration.GeocodeLimit; }
        }

        /// <summary>
        /// Gets the all the settings
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Settings
        {
            get
            {
                return _configuration.Settings.GetSettings().Select(x => new KeyValuePair<string, string>(x.Key, x.Value));
            }
        }
    }
}