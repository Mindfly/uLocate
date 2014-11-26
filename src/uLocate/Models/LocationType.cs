namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The location type definition.
    /// </summary>
    internal class LocationType : UpdateableEntity, ILocationType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationType"/> class.
        /// </summary>
        public LocationType()
            : this(new CustomFieldsCollection())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationType"/> class.
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The custom fields collection
        /// </exception>
        internal LocationType(CustomFieldsCollection fields)
        {
            if (fields == null) throw new ArgumentNullException("fields");

            Fields = fields;
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        public CustomFieldsCollection Fields { get; internal set; }
    }
}