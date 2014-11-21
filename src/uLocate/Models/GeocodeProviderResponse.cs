namespace uLocate.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Geocode Provider Response
    /// </summary>
    public class GeocodeProviderResponse : IGeocodeProviderResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodeProviderResponse"/> class.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <param name="geocodes">
        /// The geocodes.
        /// </param>
        public GeocodeProviderResponse(GeocodeStatus status, IEnumerable<IGeocode> geocodes)
        {
            Status = status;

            Results = geocodes;
        }

        /// <summary>
        /// Gets the status of the geocode request
        /// </summary>
        public GeocodeStatus Status { get; private set; }

        /// <summary>
        /// Gets the results.
        /// </summary>
        public IEnumerable<IGeocode> Results { get; private set; }
    }
}