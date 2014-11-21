namespace uLocate.Models
{
    /// <summary>
    /// The geocode status.
    /// </summary>
    /// <remarks>
    /// Enumeration of Statuses taken from the Google Geocoding API
    /// </remarks>
    public enum GeocodeStatus
    {
        /// <summary>
        /// Signifies a successful geocode query
        /// </summary>
        Ok,

        /// <summary>
        /// Signifies the query did not return any results
        /// </summary>
        ZeroResults,

        /// <summary>
        /// Signifies that the request was denied
        /// </summary>
        RequestDenied,

        /// <summary>
        /// Signifies that the request was invalid
        /// </summary>
        InvalidRequest,

        /// <summary>
        /// Signifies that nothing was queried
        /// </summary>
        NotQueried,

        /// <summary>
        /// Signifies the API has exceeded it's request limit
        /// </summary>
        OverQueryLimit,

        /// <summary>
        /// Signifies the remote API returned an error
        /// </summary>
        Error
    }
}