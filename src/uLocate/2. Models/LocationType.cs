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
    public class LocationType : EntityBase 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationType"/> class.
        /// </summary>
        public LocationType()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            Icon = Constants.BaseLocationTypeIcon;
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

        ///// <summary>
        ///// Gets or sets the Key
        ///// </summary>
        public override Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the custom properties.
        /// </summary>
        public List<LocationTypeProperty> Properties { get; set; }

        ///// <summary>
        ///// Gets or sets the create date.
        ///// </summary>
        //public DateTime CreateDate { get; set; }

        ///// <summary>
        ///// Gets or sets the update date.
        ///// </summary>
        //public DateTime UpdateDate { get; set; }

        #endregion

        #region Public Methods

        public void AddProperty(string Alias, string DisplayName, int DataTypeId, int SortOrder = 0)
        {
            var NewProp = new LocationTypeProperty();
            NewProp.LocationTypeKey = this.Key;
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

            Repositories.LocationTypePropertyRepo.Insert(NewProp);

            this.Properties.Add(NewProp);
        }

        /// <summary>
        /// Utility method used to set values when the entity is about to be created
        /// </summary>
        public override void AddingEntity()
        {
            base.AddingEntity();
            Icon = Constants.BaseLocationTypeIcon;
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