namespace uLocate.Models
{
    using System;
    using System.Linq;

    using uLocate.Data;
    using uLocate.Persistance;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location type property definition.
    /// </summary>
    public class LocationTypeProperty : EntityBase //, ILocationTypeProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationTypeProperty"/> class and sets some default values.
        /// </summary>
        public LocationTypeProperty()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            IsDefaultProp = false;
            Key = Guid.NewGuid();
        }

        #region Public Properties

     
        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        public override Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the Alias for the property
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the Display Name for the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the related Location Type for the property
        /// </summary>
        public Guid LocationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a default property
        /// </summary>
        public bool IsDefaultProp { get; set; }

        /// <summary>
        /// Gets or sets the sort order for the property
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the related DataType (from the umbraco DataTypes) for the property
        /// </summary>
        public int DataTypeId { get; set; }

        public CmsDataType DataType { get; internal set; }

        ///// <summary>
        ///// Gets the property editor alias for the associated datatype
        ///// </summary>
        //public string PropertyEditorAlias { get; internal set; }

        /// <summary>
        /// Gets the database type for the associated datatype.
        /// </summary>
        //public string DatabaseType {
        //    get
        //    {

        //    }
        //}

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        

        #endregion
    }
}
