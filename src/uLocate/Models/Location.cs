namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Persistance;

    using umbraco.presentation.webservices;

    /// <summary>
    /// Represents a Location
    /// </summary>
    public class Location : EntityBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        public Location()
        {
            this.CreateNewLocation(string.Empty, Constants.DefaultLocationTypeKey);
        }

        public Location(string LocName)
        {
            this.CreateNewLocation(LocName, Constants.DefaultLocationTypeKey);
        }

        public Location(string LocName, Guid LocTypeKey)
        {
            this.CreateNewLocation(LocName, LocTypeKey);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the key.
        /// </summary>
        public override Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location type key.
        /// </summary>
        public Guid LocationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the geocode status.
        /// </summary>
        public GeocodeStatus GeocodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the viewport.
        /// </summary>
        public Viewport Viewport { get; set; }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>
        public ICoordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the custom fields collection.
        /// </summary>
        public IEnumerable<LocationPropertyData> PropertyData { get; internal set; }

        /// <summary>
        /// Gets or sets the location type.
        /// </summary>
        public LocationType LocationType { get; set; }

        /// <summary>
        /// Gets the address.
        /// </summary>
        public Address Address
        {
            get
            {
                var add = new Address();
                add.Address1 = this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.Address1).Value.ToString();
                add.Address2 = this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.Address2).Value.ToString();
                add.CountryCode = this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.CountryCode).Value.ToString();
                add.Locality = this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.Locality).Value.ToString();
                add.PostalCode = this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.PostalCode).Value.ToString();
                add.Region = this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.Region).Value.ToString();
                return add;
            }

        }

        /// <summary>
        /// Gets the phone number
        /// </summary>
        public string Phone
        {
            get
            {
                return this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.Phone).Value.ToString();
            }
        }

        /// <summary>
        /// Gets the Email
        /// </summary>
        public string Email
        {
            get
            {
                return this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.Email).Value.ToString();
            }
        }

        /// <summary>
        /// Gets the custom fields data as a simple dictionary
        /// </summary>
        public Dictionary<string, string> CustomProperties 
        {
            get
            {
                Dictionary<string, string> PropData = new Dictionary<string, string>();

                foreach (var prop in this.PropertyData)
                {
                    if (!prop.PropertyAttributes.IsDefaultProp)
                    {
                        PropData.Add(prop.PropertyAlias, prop.Value.ToString());
                    }
                }

                return PropData;
            }
        }

        #endregion

        #region Private Methods

        private void CreateNewLocation(string LocName, Guid LocTypeKey)
        {
            this.UpdateDate = DateTime.Now;
            this.CreateDate = DateTime.Now;
            this.Key = Guid.NewGuid();
            this.Name = LocName;
            this.LocationTypeKey = LocTypeKey;
            this.LocationType = Repositories.LocationTypeRepo.GetByKey(LocTypeKey);
            this.PropertyData = this.DefaultProperties(LocTypeKey);
        }

        private IEnumerable<LocationPropertyData> DefaultProperties()
        {
            return DefaultProperties(Constants.DefaultLocationTypeKey);
        }

        private IEnumerable<LocationPropertyData> DefaultProperties(Guid LocTypeKey)
        {
            List<LocationPropertyData> NewData = new List<LocationPropertyData>();
            var DefaultProps = Repositories.LocationTypePropertyRepo.GetByLocationType(Constants.DefaultLocationTypeKey);
            foreach (var typeProperty in DefaultProps)
            {
                var NewProp = new LocationPropertyData(this.Key, typeProperty.Key);
                NewData.Add(NewProp);
            }

            //custom type props
            if (LocTypeKey != Constants.DefaultLocationTypeKey)
            {
                var CustomProps = Repositories.LocationTypePropertyRepo.GetByLocationType(LocTypeKey);
                foreach (var typeProperty in CustomProps)
                {
                    var NewProp = new LocationPropertyData(this.Key, typeProperty.Key);
                    NewData.Add(NewProp);
                }
            }

            return NewData;
        }
        #endregion


    }

}