namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using uLocate.Data;
    using uLocate.Persistance;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    using UmbracoExamine.DataServices;

    /// <summary>
    /// Property data for a Location
    /// </summary>
    [TableName("uLocate_LocationPropertyData")]
    [PrimaryKey("Id")]
    [ExplicitColumns]
    public class LocationPropertyData : EntityBase
    {
        internal LocationPropertyData()
        {
            this.CreateNewPropData(Guid.Empty, Guid.Empty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPropertyData"/> class and sets some default values.
        /// </summary>
        public LocationPropertyData(Guid LocationKey)
        {
            this.CreateNewPropData(LocationKey, Guid.Empty);
        }

        public LocationPropertyData(Guid LocationKey, Guid PropertyKey)
        {
            this.CreateNewPropData(LocationKey, PropertyKey);
        }

        private void CreateNewPropData(Guid LocationKey, Guid PropertyKey)
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            this.Key = Guid.NewGuid();
            this.LocationKey = LocationKey;
            this.LocationTypePropertyKey = PropertyKey;
            
        }

        /// <summary>
        /// Gets or sets the id for the location property data
        /// </summary>
        [Column("Key")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public override Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the related Location key
        /// </summary>
        [Column("LocationKey")]
        [ForeignKey(typeof(Location), Name = "FK_uLocateLocationPropertyData_uLocateLocation", Column = "Key")]
        public Guid LocationKey { get; set; }

        /// <summary>
        /// Gets or sets the related LocationTypeProperty
        /// </summary>
        [Column("LocationTypePropertyKey")]
        [ForeignKey(typeof(LocationTypeProperty), Name = "FK_uLocateLocationPropertyData_uLocateLocationTypeProperty", Column = "Key")]
        public Guid LocationTypePropertyKey { get; set; }

        /// <summary>
        /// Gets or sets int data for the location property
        /// </summary>
        [Column("dataInt")]
        public int dataInt { get; set; }

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
        /// Gets or sets the update date.
        /// </summary>
        [Column("UpdateDate")]
        [Constraint(Default = "getdate()")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets the attributes of the associated LocationTypeProperty
        /// </summary>
        public LocationTypeProperty PropertyAttributes
        {
            get
            {
                if (this.LocationTypePropertyKey == Guid.Empty)
                {
                    return new LocationTypeProperty();
                }
                else
                {
                    return Repositories.LocationTypePropertyRepo.GetByKey(this.LocationTypePropertyKey);
                }
            }
        }

        public string PropertyAlias
        {
            get
            {
                return this.PropertyAttributes.Alias;
            }
        }

        /// <summary>
        /// Gets the value of the data
        /// </summary>
        public PropertyValue Value
        {
            get
            {
                if (this.LocationTypePropertyKey == Guid.Empty)
                {
                    return new PropertyValue();
                }
                else
                {
                    return new PropertyValue(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        [Column("CreateDate")]
        [Constraint(Default = "getdate()")]
        public DateTime CreateDate { get; set; }

    }

    /// <summary>
    /// Handles returning the actual data in a strongly-typed format
    /// </summary>
    public class PropertyValue
    {
        #region Private Vars
        /// <summary>
        /// The _data string.
        /// </summary>
        private string _dataString;

        /// <summary>
        /// The _data int.
        /// </summary>
        private int _dataInt;

        /// <summary>
        /// The _data date.
        /// </summary>
        private DateTime _dataDate;

        /// <summary>
        /// The _data object.
        /// </summary>
        private object _dataObject;
#endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValue"/> class which is blank.
        /// </summary>
        public PropertyValue()
        {
            this.Type = ValueType.Null;
            _dataObject = null;
            // this.Value = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValue"/> class using actual LocationPropertyData
        /// </summary>
        /// <param name="PropertyData">
        /// The property data.
        /// </param>
        public PropertyValue(LocationPropertyData PropertyData)
        {
            switch (PropertyData.PropertyAttributes.DatabaseType)
            {
                case "Ntext":
                    this._dataString = PropertyData.dataNtext;
                    _dataObject = PropertyData.dataNtext;
                    this.Type = ValueType.String;
                    break;
                case "Nvarchar":
                    this._dataString = PropertyData.dataNvarchar;
                    _dataObject = PropertyData.dataNvarchar;
                    this.Type = ValueType.String;
                    break;
                case "Integer":
                    this._dataInt = PropertyData.dataInt;
                    _dataObject = PropertyData.dataInt;
                    this.Type = ValueType.Int;
                    break;
                case "Date":
                    this._dataDate = PropertyData.dataDate;
                    _dataObject = PropertyData.dataDate;
                    this.Type = ValueType.Date;
                    break;
            }
        }

        #region Public Props
        /// <summary>
        /// Value type options
        /// </summary>
        public enum ValueType
        {
            String,
            Date,
            Int,
            Null
        }

        /// <summary>
        /// Gets the type of the value
        /// </summary>
        public ValueType Type { get; internal set; }

        
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the Value as a string
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            if (_dataObject == null)
            {
                return "";
            }
            else
            {
                return _dataObject.ToString();
            }
        }

        /// <summary>
        /// Returns the Value as an int
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int ToInt()
        {
            if (this.Type == ValueType.Int & _dataObject != null)
            {
                return _dataInt;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the Value as a DateTime 
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public DateTime ToDateTime()
        {
            if (this.Type == ValueType.Date)
            {
                return _dataDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        #endregion
    }
}
