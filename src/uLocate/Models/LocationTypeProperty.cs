namespace uLocate.Models
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location type property definition.
    /// </summary>
    public class LocationTypeProperty : EntityBase //, ILocationTypeProperty
    {
        /// <summary>
        /// Gets or sets the Primary Key ID for the property
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Alias for the property
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the Display Name for the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the related DataType (from the umbraco DataTypes) for the property
        /// </summary>
        public int DataTypeId { get; set; }

        /// <summary>
        /// Gets or sets the related Location Type for the property
        /// </summary>
        public int LocationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sort order for the property
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets the property editor alias for the associated datatype
        /// </summary>
        public string PropertyEditorAlias { get; internal set; }

        /// <summary>
        /// Gets the database type for the associated datatype.
        /// </summary>
        public string DatabaseType { get; internal set; } 

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
        public override string EntityIdType
        {
            get
            {
                return "int";
            }
        }
    }
}
