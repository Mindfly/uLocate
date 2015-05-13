namespace uLocate.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a LocationType
    /// </summary>
    public interface ILocationType : IEntity
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the custom properties associated with the location type.
        /// </summary>
        IEnumerable<LocationTypeProperty> Properties { get; } 
    }
}