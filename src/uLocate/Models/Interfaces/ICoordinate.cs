namespace uLocate.Models
{
    /// <summary>
    /// Defines a coordinate (x, y)
    /// </summary>
    public interface ICoordinate
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        double Longitude { get; set; }
    }
}