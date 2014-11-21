namespace uLocate.Models.Rdbms
{
    using System;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    [TableName("ulocateLocation")]
    [PrimaryKey("pk", autoIncrement = false)]
    [ExplicitColumns]
    internal class LocationDto
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [Column("pk")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        [Constraint(Default = "newid()")]
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("name")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>
        [Column("coordinate")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Coordinate { get; set; }

        /// <summary>
        /// Gets or sets the geocode status.
        /// </summary>
        [Column("geocodeStatus")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string GeocodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the viewport.
        /// </summary>
        [Column("viewport")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Viewport { get; set; }

        /// <summary>
        /// Gets or sets the location type key.
        /// </summary>
        [Column("locationTypeKey")]
        [ForeignKey(typeof(LocationTypeDefinitionDto), Name = "FK_ulocateLocation_ulocateLocationTypeDefinition", Column = "pk")]
        public Guid LocationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the field values (JSON)
        /// </summary>
        [Column("fieldValues")]
        [NullSetting(NullSetting = NullSettings.Null)]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string FieldValues { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        [Column("createDate")]
        [Constraint(Default = "getdate()")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        [Column("updateDate")]
        [Constraint(Default = "getdate()")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the location type definition dto.
        /// </summary>
        [ResultColumn]
        public LocationTypeDefinitionDto LocationTypeDefinitionDto { get; set; } 
    }
}