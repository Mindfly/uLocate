namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Persistance;

    using umbraco.editorControls.SettingControls.Pickers;
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
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the database 'geography' field needs to be updated.
        /// </summary>
        public bool DbGeogNeedsUpdated { get; set; }

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
        public List<LocationPropertyData> PropertyData { get; internal set; }

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

        //public string DbGeog
        //{
        //    get
        //    {
                
        //        return this.PropertyData.FirstOrDefault(p => p.PropertyAlias == Constants.DefaultLocPropertyAlias.Email).Value.ToString();
        //    }
        //}

        #endregion

        #region Public Methods

        /// <summary>
        /// Add property data to this location
        /// </summary>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="PropertyValue">
        /// The property value.
        /// </param>
        public void AddPropertyData(string PropertyAlias, string PropertyValue)
        {
            var NewProp = CreateNewProp(PropertyAlias);
            NewProp.dataNvarchar = PropertyValue;
            this.PropertyData.Add(NewProp);
        }

        /// <summary>
        /// Add property data to this location
        /// </summary>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="PropertyValue">
        /// The property value.
        /// </param>
        public void AddPropertyData(string PropertyAlias, int PropertyValue)
        {
            var NewProp = CreateNewProp(PropertyAlias);
            NewProp.dataInt = PropertyValue;
            this.PropertyData.Add(NewProp);
        }

        /// <summary>
        /// Add property data to this location
        /// </summary>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="PropertyValue">
        /// The property value.
        /// </param>
        public void AddPropertyData(string PropertyAlias, DateTime PropertyValue)
        {
            var NewProp = CreateNewProp(PropertyAlias);
            NewProp.dataDate = PropertyValue;
            this.PropertyData.Add(NewProp);
        }

        /// <summary>
        /// Add property data to this location
        /// </summary>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="PropertyValue">
        /// The property value.
        /// </param>
        public void AddPropertyData(string PropertyAlias, object PropertyValue)
        {
            Guid LocationKey = this.Key;
            var thisProperty = Repositories.LocationTypePropertyRepo.GetByAlias(PropertyAlias);

            var existingPropertyData = Repositories.LocationPropertyDataRepo.GetByAlias(PropertyAlias, LocationKey);

            if (existingPropertyData != null)
            {
                //Update Prop
                switch (thisProperty.DatabaseType)
                {
                    case Constants.DbNtext:
                        existingPropertyData.dataNtext = PropertyValue.ToString();
                        break;
                    case Constants.DbNvarchar:
                        existingPropertyData.dataNvarchar = PropertyValue.ToString();
                        break;
                    case Constants.DbInteger:
                        existingPropertyData.dataInt = Convert.ToInt32(PropertyValue);
                        break;
                    case Constants.DbDate:
                        existingPropertyData.dataDate = Convert.ToDateTime(PropertyValue);
                        break;
                }

                Repositories.LocationPropertyDataRepo.Update(existingPropertyData);

            }
            else
            {
                //Add new prop
                var NewPropData = CreateNewProp(PropertyAlias);

                switch (thisProperty.DatabaseType)
                {
                    case Constants.DbNtext:
                        NewPropData.dataNtext = PropertyValue.ToString();
                        break;
                    case Constants.DbNvarchar:
                        NewPropData.dataNvarchar = PropertyValue.ToString();
                        break;
                    case Constants.DbInteger:
                        NewPropData.dataInt = Convert.ToInt32(PropertyValue);
                        break;
                    case Constants.DbDate:
                        NewPropData.dataDate = Convert.ToDateTime(PropertyValue);
                        break;
                }

                this.PropertyData.Add(NewPropData);
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
            this.PropertyData = this.DefaultProperties(LocTypeKey).ToList();
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

        private LocationPropertyData CreateNewProp(string PropertyAlias)
        {
            var locTypeProp = this.LocationType.Properties.FirstOrDefault(p => p.Alias == PropertyAlias);

            if (locTypeProp != null)
            {
                var NewProp = new LocationPropertyData();
                NewProp.LocationKey = this.Key;
                NewProp.LocationTypePropertyKey = locTypeProp.Key;
                return NewProp;
            }
            else
            {
                throw new Exception("Provided property alias does not match a valid property for this location type.");
            }
        }
        #endregion

        
    }

}