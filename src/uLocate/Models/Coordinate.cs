namespace uLocate.Models
{
    /// <summary>
    /// Represents a coordinate.
    /// </summary>
    public class Coordinate : ICoordinate
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }
    }
}