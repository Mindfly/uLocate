namespace uLocate.Models
{
    using System.Linq;

    /// <summary>
    /// Utility extension methods
    /// </summary>
    public static class UtilityExtensions
    {

        #region ICoordinate

        /// <summary>
        /// The is zero zero.
        /// </summary>
        /// <param name="coordinate">
        /// The coordinate.
        /// </param>
        /// <returns>
        /// True if the coordinate is 0,0 otherwise returns false.
        /// </returns>
        public static bool IsZeroZero(this ICoordinate coordinate)
        {
            return coordinate.Latitude.Equals(0) && coordinate.Longitude.Equals(0);
        }


        #endregion

        #region IAddress

        ///// <summary>
        ///// Adds the Geocode information to an <see cref="IAddress"/> if possible
        ///// </summary>
        ///// <param name="address">
        ///// The address.
        ///// </param>
        ///// <param name="response">
        ///// The response.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IAddress"/>.
        ///// </returns>
        //public static IAddress AddGeocodeData(this IAddress address, GeocodeProviderResponse response)
        //{
        //    if (response.Status != GeocodeStatus.Ok) return address;
        //    if (!response.Results.Any()) return address;

        //    var result = response.Results.First();

        //    address.Viewport = result.Viewport;
        //    address.Coordinate = new Coordinate() { Latitude = result.Latitude, Longitude = result.Longitude };

        //    return address;
        //}

        /// <summary>
        /// Creates a string used for geocoding requests
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <returns>
        /// The address formatted as a single string.
        /// </returns>
        public static string AsApiRequestFormattedAddressString(this IAddress address)
        {
            return GetApiRequestFormattedAddressString(address.Address1, address.Address2, address.Locality, address.Region, address.PostalCode, address.CountryCode);
        }

        /// <summary>
        /// Creates an address formatted as a single string.
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
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetApiRequestFormattedAddressString(string address1, string address2, string locality, string region, string postalCode, string countryCode)
        {
            var segments = new[]
            {
                string.Concat(address1, " ", address2),
                locality ?? string.Empty,
                string.Concat(region, " ", postalCode),
                countryCode ?? string.Empty
            };

            return string.Join(", ", segments.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        #endregion

    }
}