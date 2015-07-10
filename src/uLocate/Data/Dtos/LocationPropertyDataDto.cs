namespace uLocate.Data
{
    using System;

    using uLocate.Models;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// Dto for the "uLocate_LocationPropertyData" table
    /// </summary>
    [TableName("uLocate_LocationPropertyData")]
    [PrimaryKey("Key", autoIncrement = false)]
    [ExplicitColumns]
    internal class LocationPropertyDataDto
    {
        /// <summary>
        /// Gets or sets the id for the location property data
        /// </summary>
        [Column("Key")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        [Constraint(Default = "newid()")]
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the related EditableLocation key
        /// </summary>
        [Column("LocationKey")]
        [ForeignKey(typeof(LocationDto), Name = "FK_uLocateLocationPropertyData_uLocateLocation", Column = "Key")]
        public Guid LocationKey { get; set; }

        /// <summary>
        /// Gets or sets the related LocationTypeProperty
        /// </summary>
        [Column("LocationTypePropertyKey")]
        [ForeignKey(typeof(LocationTypePropertyDto), Name = "FK_uLocateLocationPropertyData_uLocateLocationTypeProperty", Column = "Key")]
        public Guid LocationTypePropertyKey { get; set; }

        /// <summary>
        /// Gets or sets int data for the location property
        /// </summary>
        [Column("dataInt")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int dataInt { get; set; }

        /// <summary>
        /// Gets or sets date data for the location property
        /// </summary>
        [Column("dataDate")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public DateTime dataDate { get; set; }

        /// <summary>
        /// Gets or sets nvarchar data for the location property
        /// </summary>
        [Column("dataNvarchar")]
        [Length(500)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string dataNvarchar { get; set; }

        /// <summary>
        /// Gets or sets ntext data for the location property
        /// </summary>
        [Column("dataNtext")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string dataNtext { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        [Column("CreateDate")]
        [Constraint(Default = "getdate()")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        [Column("UpdateDate")]
        [Constraint(Default = "getdate()")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPropertyDataDto"/> class and sets some default values.
        /// </summary>
        public LocationPropertyDataDto()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            Key = Guid.NewGuid();
        }

    }
}

