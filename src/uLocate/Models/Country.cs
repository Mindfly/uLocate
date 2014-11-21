namespace uLocate.Models
{
    /// <summary>
    /// Represents a country.
    /// </summary>
    public class Country : ICountry
    {
        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode { get; internal set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; internal set; }
    }
}