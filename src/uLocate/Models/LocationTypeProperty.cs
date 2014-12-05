namespace uLocate.Models
{
    using System;

    using uLocate.Data;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location type property definition.
    /// </summary>
    [TableName("uLocate_LocationTypeProperty")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public class LocationTypeProperty : EntityBase //, ILocationTypeProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationTypeProperty"/> class and sets some default values.
        /// </summary>
        public LocationTypeProperty()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
        }

        #region Public Properties

     
        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        [Column("Key")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public override Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the Alias for the property
        /// </summary>
        [Column("Alias")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the Display Name for the property
        /// </summary>
        [Column("Name")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the related Location Type for the property
        /// </summary>
        [Column("LocationTypeKey")]
        [ForeignKey(typeof(LocationType), Name = "FK_uLocateLocationTypeProperty_uLocateLocationType", Column = "Key")]
        public Guid LocationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the related DataType (from the umbraco DataTypes) for the property
        /// </summary>
        [Column("DataTypeId")]
        [ForeignKey(typeof(cmsDataTypeDto), Name = "FK_uLocateLocationTypeProperty_cmsDataType", Column = "nodeId")]
        public int DataTypeId { get; set; }

        /// <summary>
        /// Gets the property editor alias for the associated datatype
        /// </summary>
        public string PropertyEditorAlias { get; internal set; }

        /// <summary>
        /// Gets the database type for the associated datatype.
        /// </summary>
        public string DatabaseType { get; internal set; }

        /// <summary>
        /// Gets or sets the sort order for the property
        /// </summary>
        [Column("sortOrder")]
        public int SortOrder { get; set; }

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





        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
