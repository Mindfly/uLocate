namespace uLocate.Models
{
    /// <summary>
    /// Represents a geo spacial "geocode"
    /// </summary>
    public class Geocode : IGeocode
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the formatted address.
        /// </summary>
        public string FormattedAddress { get; set; }

        /// <summary>
        /// Gets or sets the quality of the geocode data
        /// </summary>
        public GeocodeQuality Quality { get; set; }

        /// <summary>
        /// Gets or sets the viewport - used by some mapping providers
        /// </summary>
        public IViewport Viewport { get; set; }
    }
}