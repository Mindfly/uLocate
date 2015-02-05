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
    public class LocationPropertyData : EntityBase
    {

        #region Constructors

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

        #endregion

        #region Internal Properties

        #endregion

        #region Public Properties/Enums

        //public enum DbType
        //{
        //    Ntext,
        //    Nvarchar,
        //    Integer,
        //    Date,
        //    Unknown
        //}

        /// <summary>
        /// Gets or sets the id for the location property data
        /// </summary>
        public override Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the related Location key
        /// </summary>
        public Guid LocationKey { get; set; }

        /// <summary>
        /// Gets or sets the related LocationTypeProperty
        /// </summary>
        public Guid LocationTypePropertyKey { get; set; }

        /// <summary>
        /// Gets or sets int data for the location property
        /// </summary>
        internal int dataInt { get; set; }

        /// <summary>
        /// Gets or sets date data for the location property
        /// </summary>
        internal DateTime dataDate { get; set; }

        /// <summary>
        /// Gets or sets nvarchar data for the location property
        /// </summary>
        internal string dataNvarchar { get; set; }

        /// <summary>
        /// Gets or sets ntext data for the location property
        /// </summary>
        internal string dataNtext { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
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

        public CmsDataType.DbType DatabaseType
        {
            get
            {
                return this.PropertyAttributes.DataType.DatabaseType;
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
        public DateTime CreateDate { get; set; }

        #endregion

        #region Internal/Public Methods

        internal void SetValue(string PropertyValue)
        {
            switch (this.DatabaseType)
            {
                case CmsDataType.DbType.Date:
                    this.dataDate = DateTime.Parse(PropertyValue);
                    break;
                case CmsDataType.DbType.Integer:
                    this.dataInt = Convert.ToInt32(PropertyValue);
                    break;
                case CmsDataType.DbType.Ntext:
                    this.dataNtext = PropertyValue;
                    break;
                case CmsDataType.DbType.Nvarchar:
                    this.dataNvarchar = PropertyValue;
                    break;
            }

            Repositories.LocationPropertyDataRepo.Update(this);
        }

        internal void SetValue(int PropertyValue)
        {
            switch (this.DatabaseType)
            {
                case CmsDataType.DbType.Date:
                    this.dataDate = DateTime.MinValue;
                    break;
                case CmsDataType.DbType.Integer:
                    this.dataInt = PropertyValue;
                    break;
                case CmsDataType.DbType.Ntext:
                    this.dataNtext = PropertyValue.ToString();
                    break;
                case CmsDataType.DbType.Nvarchar:
                    this.dataNvarchar = PropertyValue.ToString();
                    break;
            }

            Repositories.LocationPropertyDataRepo.Update(this);
        }

        internal void SetValue(DateTime PropertyValue)
        {
            switch (this.DatabaseType)
            {
                case CmsDataType.DbType.Date:
                    this.dataDate = PropertyValue;
                    break;
                case CmsDataType.DbType.Integer:
                    this.dataInt = 0;
                    break;
                case CmsDataType.DbType.Ntext:
                    this.dataNtext = PropertyValue.ToShortDateString();
                    break;
                case CmsDataType.DbType.Nvarchar:
                    this.dataNvarchar = PropertyValue.ToShortDateString();
                    break;
            }

            Repositories.LocationPropertyDataRepo.Update(this);
        }

        internal void SetValue(object PropertyValue)
        {
            switch (this.DatabaseType)
            {
                case CmsDataType.DbType.Integer:
                    this.dataInt = Convert.ToInt32(PropertyValue);
                    break;
                case CmsDataType.DbType.Date:
                    this.dataDate = Convert.ToDateTime(PropertyValue);
                    break;
                case CmsDataType.DbType.Ntext:
                    this.dataNtext = PropertyValue.ToString();
                    break;
                case CmsDataType.DbType.Nvarchar:
                    this.dataNvarchar = PropertyValue.ToString();
                    break;
            }

            Repositories.LocationPropertyDataRepo.Update(this);
        }


        #endregion

        #region Private Methods


        private void CreateNewPropData(Guid LocationKey, Guid PropertyKey)
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            this.Key = Guid.NewGuid();
            this.LocationKey = LocationKey;
            this.LocationTypePropertyKey = PropertyKey;

        }

        

        #endregion


   
    }
}
