namespace uLocate.Data
{
    using System;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location type dto - used for creating/deleting the table
    /// </summary>
    [TableName("uLocate_LocationType")]
    [PrimaryKey("Key", autoIncrement = false)]
    [ExplicitColumns] 
    internal class LocationTypeDto
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [Column("Key")]
        [PrimaryKeyColumn(AutoIncrement = false)]
        [Constraint(Default = "newid()")]
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the Location name.
        /// </summary>
        [Column("Name")]
        [Length(150)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Column("Description")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        [NullSetting(NullSetting = NullSettings.Null)]

        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        [Column("Icon")]
        [Length(100)]
        public string Icon { get; set; }

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
        /// Initializes a new instance of the <see cref="LocationTypeDto"/> class and sets some default values.
        /// </summary>
        public LocationTypeDto()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            Key = Guid.NewGuid();
            Icon = Constants.BaseLocationTypeIcon;
        }
    }
}
