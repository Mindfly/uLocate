namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using uLocate.Data;
    using uLocate.Persistance;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location type definition.
    /// </summary>
    [TableName("uLocate_LocationType")]
    [PrimaryKey("Id")]
    [ExplicitColumns] 
    public class LocationType : EntityBase //, ILocationType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationType"/> class.
        /// </summary>
        public LocationType()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            this.Properties = new List<LocationTypeProperty>();
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LocationType"/> class.
        ///// </summary>
        ///// <param name="LocationTypeId">
        ///// The Location Type Id.
        ///// </param>
        //internal LocationType(int LocationTypeId)
        //{
        //    UpdateDate = DateTime.Now;
        //    CreateDate = DateTime.Now;
        //}

        #region Public Properties

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name")]
        [Length(150)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the custom properties.
        /// </summary>
        public List<LocationTypeProperty> Properties { get; set; }

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
        /// Gets the id key.
        /// </summary>
        public override object IdKey
        {
            get
            {
                return this.Id;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the entity has a valid id
        /// </summary>
        public override bool HasIdentity
        {
            get
            {
                return !Id.Equals(0) & !Id.Equals(null);
            }
        }

        /// <summary>
        /// Gets the entity id type (int)
        /// </summary>
        public override string EntityIdType
        {
            get
            {
                return "int";
            }
        }

        #endregion

        #region Public Methods

        public void AddProperty(string Alias, string DisplayName, int DataTypeId, int SortOrder = 0)
        {
            var NewProp = new LocationTypeProperty();
            NewProp.LocationTypeId = this.Id;
            NewProp.Alias = Alias;
            NewProp.Name = DisplayName;
            NewProp.DataTypeId = DataTypeId;

            if (SortOrder != 0)
            {
                NewProp.SortOrder = SortOrder;
            }
            else
            {
                NewProp.SortOrder = PropMaxSortNum() + 1;
            }

            this.Properties.Add(NewProp);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the Max Sort order for this Location type's properties
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int PropMaxSortNum()
        {
            int Max = 0;

            if (this.Properties.Any())
            {
                var SortedProps = this.Properties.OrderBy(p => p.SortOrder);
                Max = SortedProps.Last().SortOrder;
            }

            return Max;
        }

        #endregion

    }
}