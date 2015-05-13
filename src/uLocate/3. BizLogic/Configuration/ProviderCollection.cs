namespace uLocate.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The provider collection.
    /// </summary>
    public class ProviderCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Default. Returns the ProviderElement with the index of index from the collection
        /// </summary>
        public ProviderElement this[object index]
        {
            get { return (ProviderElement)BaseGet(index); }
        }

        /// <summary>
        /// Creates a new <see cref="ConfigurationElement">ConfigurationElement</see>.
        /// CreateNewElement must be overridden in classes that derive from the ConfigurationElementCollection class.
        /// </summary>
        /// <returns>
        /// The <see cref="ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProviderElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">
        /// DataTypeElement
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProviderElement)element).Alias;
        }        
    }
}