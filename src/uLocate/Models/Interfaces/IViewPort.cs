namespace uLocate.Models
{
    /// <summary>
    /// Defines the best recommended "viewport" to display a result set in some map APIs.
    /// </summary>
    public interface IViewport
    {
        /// <summary>
        /// Gets or sets the south west corner of the viewport
        /// </summary>
        ICoordinate SouthWest { get; set; }

        /// <summary>
        /// Gets or sets the north east corner of the viewport
        /// </summary>
        ICoordinate NorthEast { get; set; }
    }
}