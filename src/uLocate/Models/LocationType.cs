namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using uLocate.Data;
    using uLocate.Persistance;

    /// <summary>
    /// The location type definition.
    /// </summary>
    public class LocationType : EntityBase //, ILocationType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationType"/> class.
        /// </summary>
        public LocationType()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationType"/> class.
        /// </summary>
        /// <param name="LocationTypeId">
        /// The Location Type Id.
        /// </param>
        internal LocationType(int LocationTypeId)
        {
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        public IEnumerable<LocationTypeProperty> Properties { get; set; }


        /// <summary>
        /// Gets the id key.
        /// </summary>
        public override object IdKey
        {
            get
            {
                return this.Id;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the entity has a valid id
        /// </summary>
        public override bool HasIdentity
        {
            get
            {
                return !Id.Equals(0) & !Id.Equals(null);
            }
        }

        /// <summary>
        /// Gets the entity id type (int)
        /// </summary>
        public override string EntityIdType {
            get
            {
                return "int";
            }
        }
    }
}