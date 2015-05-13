namespace uLocate.Models
{
    /// <summary>
    /// Defines an address
    /// </summary>
    public interface IAddress
    {
        /// <summary>
        /// Gets or sets the address 1.
        /// </summary>
        string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address 2.
        /// </summary>
        string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the locality - generally the city name
        /// </summary>
        string Locality { get; set; }

        /// <summary>
        /// Gets or sets the region - generally the state or province
        /// </summary>
        string Region { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the two letter ISO country code.
        /// </summary>
        string CountryCode { get; set; }

    }
}