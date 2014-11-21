namespace uLocate.Models
{
    /// <summary>
    /// Represents a simple address model
    /// </summary>
    public class Address : IAddress
    {
        /// <summary>
        /// Gets or sets the street address line 1.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the street address line 2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the locality.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string CountryCode { get; set; }
    }
}