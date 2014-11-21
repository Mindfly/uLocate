namespace uLocate.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The provider element.
    /// </summary>
    public class ProviderElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the alias (key) value for the provider collection element
        /// </summary>
        /// <remarks>
        /// This assoicates the configuration with the provider after resolution
        /// </remarks>
        [ConfigurationProperty("alias", IsKey = true)]
        public string Alias
        {
            get { return (string)this["alias"]; }
        }

        /// <summary>
        /// Gets a value indicating whether or not requests should be cached in the configured runtime cache
        /// </summary>
        [ConfigurationProperty("enableCaching", IsRequired = true)]
        public bool EnableCaching
        {
            get
            {
                return (bool)this["enableCaching"];
            }
        }


        /// <summary>
        /// Gets the cacheDuration value in seconds.
        /// </summary>
        [ConfigurationProperty("cacheDuration", IsRequired = true)]
        public int CacheDuration
        {
            get { return (int)this["cacheDuration"]; }
        }

        /// <summary>
        /// Gets the geocode limit value.
        /// </summary>
        /// <remarks>
        /// Some providers restrict the number of geocoding requests can be performed
        /// </remarks>
        [ConfigurationProperty("geocodeLimit", IsRequired = false, DefaultValue = 500)]
        public int GeocodeLimit
        {
            get { return (int)this["geocodeLimit"]; }
        }

        /// <summary>
        /// Gets a value indicating whether or not geocode requests should be logged.
        /// </summary>
        /// <remarks>
        /// Logged requests will show up in the default Umbraco Logs (App_Data/Logs) as we are using the LogHelper
        /// </remarks>
        [ConfigurationProperty("logRequests", IsRequired = false, DefaultValue = false)]
        public bool LogRequests
        {
            get { return (bool)this["logRequests"]; }
        }

        /// <summary>
        /// Gets any addition settings for the provider
        /// </summary>
        [ConfigurationProperty("settings", IsRequired = false), ConfigurationCollection(typeof(SettingsCollection), AddItemName = "add")]
        public SettingsCollection Settings
        {
            get { return (SettingsCollection)this["settings"]; }
        }
    }
}
