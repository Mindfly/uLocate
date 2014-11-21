namespace uLocate.Models
{
    /// <summary>
    /// Represents the best recommended "viewport" to display a result set in some map APIs.
    /// </summary>
    public class Viewport : IViewport
    {    
        /// <summary>
        /// Gets or sets the south west corner of the viewport
        /// </summary>
        public ICoordinate SouthWest { get; set; }

        /// <summary>
        /// Gets or sets the north east corner of the viewport
        /// </summary>
        public ICoordinate NorthEast { get; set; }
    }
}