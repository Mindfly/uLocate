namespace uLocate.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The uLocate configuration section
    /// </summary>
    public class uLocateSection : ConfigurationSection 
    {
        /// <summary>
        /// Gets the providers configuration collection
        /// </summary>
        [ConfigurationProperty("providers", IsRequired = false), ConfigurationCollection(typeof(ProviderCollection), AddItemName = "provider")]
        public ProviderCollection Providers
        {
            get { return (ProviderCollection)this["providers"]; }
        }
    }
}