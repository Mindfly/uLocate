namespace uLocate.Models
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location type property definition.
    /// </summary>
    internal class LocationTypeProperty
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
        public int UmbracoDataTypeId { get; set; }

        /// <summary>
        /// Gets or sets the related Location Type for the property
        /// </summary>
        public int LocationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sort order for the property
        /// </summary>
        public int SortOrder { get; set; } 
    }
}
