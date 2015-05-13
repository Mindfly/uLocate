namespace uLocate.Models
{
    /// <summary>
    /// Represents a coordinate.
    /// </summary>
    public class Coordinate : ICoordinate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate"/> class.
        /// </summary>
        public Coordinate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate"/> class.
        /// </summary>
        /// <param name="Lat">
        /// The latitude value
        /// </param>
        /// <param name="Long">
        /// The longitude value
        /// </param>
        public Coordinate(double Lat, double Long)
        {
            this.Latitude = Lat;
            this.Longitude = Long;
        }

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