namespace uLocate.Data
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// Dto for the "uLocate_LocationPropertyData" table
    /// </summary>
    [TableName("uLocate_LocationPropertyData")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    internal class LocationPropertyDataDto
    {
        /// <summary>
        /// Gets or sets the id for the location property data
        /// </summary>
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the related Location key
        /// </summary>
        [Column("LocationKey")]
        [ForeignKey(typeof(LocationDto), Name = "FK_uLocateLocationPropertyData_uLocateLocation", Column = "Key")]
        public Guid LocationKey { get; set; }

        /// <summary>
        /// Gets or sets the related LocationTypeProperty
        /// </summary>
        [Column("LocationTypePropertyId")]
        [ForeignKey(typeof(LocationTypePropertyDto), Name = "FK_uLocateLocationPropertyData_uLocateLocationTypeProperty", Column = "Id")]
        public int LocationTypePropertyId { get; set; }

        /// <summary>
        /// Gets or sets int data for the location property
        /// </summary>
        [Column("dataInt")]
        public string dataInt { get; set; }

        /// <summary>
        /// Gets or sets date data for the location property
        /// </summary>
        [Column("dataDate")]
        public DateTime dataDate { get; set; }

        /// <summary>
        /// Gets or sets nvarchar data for the location property
        /// </summary>
        [Column("dataNvarchar")]
        [Length(500)]
        public string dataNvarchar { get; set; }

        /// <summary>
        /// Gets or sets ntext data for the location property
        /// </summary>
        [Column("dataNtext")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
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
        }
    }
}

