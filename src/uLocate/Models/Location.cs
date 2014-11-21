namespace uLocate.Models
{
    using System;

    /// <summary>
    /// Represents a Location
    /// </summary>
    public class Location : EntityBase, ILocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        public Location()
            : this(new CustomFieldsCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        /// <param name="customFields">
        /// The custom fields.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws an exception if customFields is null
        /// </exception>
        internal Location(CustomFieldsCollection customFields)
        {
            if (customFields == null) throw new ArgumentNullException("customFields");

            Fields = customFields;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location type key.
        /// </summary>
        public Guid LocationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the geocode status.
        /// </summary>
        public GeocodeStatus GeocodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the viewport.
        /// </summary>
        public IViewport Viewport { get; set; }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>
        public ICoordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the custom fields collection.
        /// </summary>
        public CustomFieldsCollection Fields { get; internal set; }

        /// <summary>
        /// Gets or sets the location type definition.
        /// </summary>
        /// <remarks>
        /// Used for validation
        /// </remarks>
        internal LocationTypeDefinition LocationTypeDefinition { get; set; }
    }
}