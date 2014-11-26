namespace uLocate.Models
{
    using System;

    /// <summary>
    /// Represents a Location
    /// </summary>
    public class Location : UpdateableEntity, ILocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        //public Location()
        //    : this()//new CustomFieldsCollection()
        //{
        //}

        ///// <summary>
        ///// Initializes a new instance of the <see cref="Location"/> class.
        ///// </summary>
        ///// <param name="customFields">
        ///// The custom fields.
        ///// </param>
        ///// <exception cref="ArgumentNullException">
        ///// Throws an exception if customFields is null
        ///// </exception>
        //internal Location()//CustomFieldsCollection customFields
        //{
        //    //if (customFields == null) throw new ArgumentNullException("customFields");

        //    //Fields = customFields;
        //}

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location type key.
        /// </summary>
        public int LocationTypeId { get; set; }

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
        internal LocationType LocationTypeDefinition { get; set; }

        /// <summary>
        /// Utility method used to update the update date when the entity is about to be created
        /// </summary>
        public override void AddingEntity()
        {
            base.AddingEntity();
            Key = Guid.NewGuid();
        }

        /// <summary>
        /// Gets a value indicating whether the entity has an identity.
        /// </summary>
        public virtual bool HasIdentity
        {
            get
            {
                return !Key.Equals(Guid.Empty);
            }
        }
    }
}