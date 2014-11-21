namespace uLocate.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The setting element.
    /// </summary>
    public class SettingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets key value for the settings collection element
        /// </summary>
        [ConfigurationProperty("key", IsKey = true)]
        public string Key
        {
            get { return (string)this["key"]; }
        }

        /// <summary>
        /// Gets the value for the settings element
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
        }
    }
}
