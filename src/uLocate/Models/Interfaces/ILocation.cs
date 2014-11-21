namespace uLocate.Models
{
    using System;

    /// <summary>
    /// Defines a LocatedAddress
    /// </summary>
    public interface ILocation : IEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the location type key.
        /// </summary>
        Guid LocationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the geocode status.
        /// </summary>
        GeocodeStatus GeocodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the recommended viewport.
        /// </summary>
        IViewport Viewport { get; set; }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>
        ICoordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the collection of custom fields.
        /// </summary>
        CustomFieldsCollection Fields { get; }
    }
}