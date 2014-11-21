namespace uLocate.Models
{
    /// <summary>
    /// Defines a geocode
    /// </summary>
    public interface IGeocode : ICoordinate
    {
        /// <summary>
        /// Gets or sets the formatted address.
        /// </summary>
        string FormattedAddress { get; set; }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        GeocodeQuality Quality { get; set; }

        /// <summary>
        /// Gets or sets the viewport.
        /// </summary>
        IViewport Viewport { get; set; }
    }
}