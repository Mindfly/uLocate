namespace uLocate.Providers
{
    using System;

    /// <summary>
    /// The geocode provider attribute.
    /// </summary>
    public class GeocodeProviderAttribute : Attribute
    {  
        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodeProviderAttribute"/> class.
        /// </summary>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <param name="name">
        /// The name gets the name of the provider
        /// </param>
        /// <exception cref="ArgumentException">
        /// Throws an <see cref="ArgumentException"/> if alias is null or empty
        /// </exception>
        public GeocodeProviderAttribute(string alias, string name)
        {
            if (string.IsNullOrEmpty(alias)) throw new ArgumentException("The alias is required");

            Alias = alias;
        }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}