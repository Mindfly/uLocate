namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
            switch (PropertyData.DatabaseType)
            {
                case LocationPropertyData.DbType.Date:
                    this._dataDate = PropertyData.dataDate;
                    _dataObject = PropertyData.dataDate;
                    this.Type = ValueType.Date;
                    break;
                case LocationPropertyData.DbType.Integer:
                    this._dataInt = PropertyData.dataInt;
                    _dataObject = PropertyData.dataInt;
                    this.Type = ValueType.Int;
                    break;
                case LocationPropertyData.DbType.Ntext:
                    this._dataString = PropertyData.dataNtext;
                    _dataObject = PropertyData.dataNtext;
                    this.Type = ValueType.String;
                    break;
                case LocationPropertyData.DbType.Nvarchar:
                    this._dataString = PropertyData.dataNvarchar;
                    _dataObject = PropertyData.dataNvarchar;
                    this.Type = ValueType.String;
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

        public object ValueObject
        {
            get
            {
                return this._dataObject;
            }
        }

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
