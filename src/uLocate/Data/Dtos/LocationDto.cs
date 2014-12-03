namespace uLocate.Data
{
    using System;

    using uLocate.Models;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    [TableName("uLocate_Location")]
    [PrimaryKey("Key", autoIncrement = false)]
    [ExplicitColumns]
    internal class LocationDto
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [Column("Key")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        [Constraint(Default = "newid()")]
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>
        [Column("Coordinate")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Coordinate { get; set; }

        /// <summary>
        /// Gets or sets the geocode status.
        /// </summary>
        [Column("GeocodeStatus")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string GeocodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the viewport.
        /// </summary>
        [Column("Viewport")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Viewport { get; set; }

        /// <summary>
        /// Gets or sets the location type key.
        /// </summary>
        [Column("LocationTypeId")]
        [ForeignKey(typeof(LocationType), Name = "FK_uLocateLocation_uLocateLocationType", Column = "Id")]
        public int LocationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the location type dto.
        /// </summary>
        [ResultColumn]
        public LocationType LocationType { get; set; } 

        ///// <summary>
        ///// Gets or sets the location type key.
        ///// </summary>
        //[Column("LocationTypePropertyDataId")]
        //[ForeignKey(typeof(LocationType), Name = "FK_uLocateLocation_uLocateLocationTypeData", Column = "Id")]
        //public int LocationTypePropertyDataId { get; set; }

        ///// <summary>
        ///// Gets or sets the field values (JSON)
        ///// </summary>
        //[Column("fieldValues")]
        //[NullSetting(NullSetting = NullSettings.Null)]
        //[SpecialDbType(SpecialDbTypes.NTEXT)]
        //public string FieldValues { get; set; }

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


    }
}