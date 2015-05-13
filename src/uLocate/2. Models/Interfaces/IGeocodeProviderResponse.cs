namespace uLocate.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the response expected from a GeocodeProvider API query
    /// </summary>
    public interface IGeocodeProviderResponse
    {
        /// <summary>
        /// Gets the status of the API Query
        /// </summary>
        GeocodeStatus Status { get; }

        /// <summary>
        /// Gets an enumeration of possible geocode results
        /// </summary>
        IEnumerable<IGeocode> Results { get; } 
    }
}