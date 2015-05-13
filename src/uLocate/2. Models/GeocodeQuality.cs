namespace uLocate.Models
{
    /// <summary>
    /// Specifies the quality of a geocode response from the API
    /// </summary>
    public enum GeocodeQuality
    {
        /// <summary>
        /// The rooftop of the locate
        /// </summary>
        Rooftop,

        /// <summary>
        /// The range interpolated.
        /// </summary>
        RangeInterpolated,

        /// <summary>
        /// The center of the locate
        /// </summary>
        Center,

        /// <summary>
        /// Approximate locate
        /// </summary>
        Approximate,

        /// <summary>
        /// The request was not located
        /// </summary>
        None
    }
}