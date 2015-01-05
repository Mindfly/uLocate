namespace uLocate.Data
{
    using System;

    using uLocate.Models;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location dto for creating/deleting the table
    /// </summary>
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

        ///// <summary>
        ///// Gets or sets the coordinate.
        ///// </summary>
        //[Column("GeoCoordinate")]
        //[NullSetting(NullSetting = NullSettings.Null)]
        //public string GeoCoordinate { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        [Column("Latitude")]
        //[NullSetting(NullSetting = NullSettings.Null)]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        [Column("Longitude")]
        //[NullSetting(NullSetting = NullSettings.Null)]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the database 'geography' field needs to be updated.
        /// </summary>
        [Column("GeogNeedsUpdated")]
        public bool DbGeogNeedsUpdated { get; set; }

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
        [Column("LocationTypeKey")]
        [ForeignKey(typeof(LocationTypeDto), Name = "FK_uLocateLocation_uLocateLocationType", Column = "Key")]
        public Guid LocationTypeKey { get; set; }

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
        /// Initializes a new instance of the <see cref="LocationDto"/> class and sets some default values.
        /// </summary>
        public LocationDto()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            Key = Guid.NewGuid();
        }
    }
}