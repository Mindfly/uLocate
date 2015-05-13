namespace uLocate.Models
{
    /// <summary>
    /// Defines a Country.
    /// </summary>
    public interface ICountry
    {
        /// <summary>
        /// Gets the two letter ISO Region code
        /// </summary>
        string CountryCode { get; }

        /// <summary>
        /// Gets the English name associated with the region
        /// </summary>
        string Name { get; }
    }
}