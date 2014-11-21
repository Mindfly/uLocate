namespace uLocate.Providers
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Defines an GeocodeProvider
    /// </summary>
    public interface IGeocodeProvider
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> Settings { get; } 

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        IGeocodeProviderResponse Geocode(IAddress address);

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        /// <param name="countryCode">
        /// The country code.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        IGeocodeProviderResponse Geocode(string postalCode, string countryCode);

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="locality">
        /// The locality.
        /// </param>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        /// <param name="countryCode">
        /// The country code.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        IGeocodeProviderResponse Geocode(string locality, string region, string postalCode, string countryCode);

        /// <summary>
        /// Queries the API for a geocode
        /// </summary>
        /// <param name="address1">
        /// The address 1.
        /// </param>
        /// <param name="address2">
        /// The address 2.
        /// </param>
        /// <param name="locality">
        /// The locality.
        /// </param>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        /// <param name="countryCode">
        /// The country code.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>
        IGeocodeProviderResponse Geocode(string address1, string address2, string locality, string region, string postalCode, string countryCode);
    }
}