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
        #region Private Vars

        private Lazy<LocationType> _RelatedLocationTypeObject = null;

        //private Lazy<IEnumerable<LocationPropertyData>> _PropertyDataObjectsList = null;

        #endregion

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
        public LocationType LocationType
        {
            get
            {
                return Repositories.LocationTypeRepo.GetByKey(this.LocationTypeKey);
            }
        }

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
        /// Add  STRING property data to this location
        /// </summary>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="PropertyValue">
        /// The property value.
        /// </param>
        public void AddPropertyData(string PropertyAlias, string PropertyValue)
        {
            var matchingPropData = this.PropertyData.Where(p => p.PropertyAlias == PropertyAlias).FirstOrDefault();

            if (matchingPropData != null)
            {
                //update existing
                matchingPropData.SetValue(PropertyValue);
            }
            else
            {
                //Add new
                var NewProp = GetOrCreatePropertyData(PropertyAlias);
                NewProp.SetValue(PropertyValue);
                this.PropertyData.Add(NewProp);
            }

        }

        /// <summary>
        /// Add INT property data to this location
        /// </summary>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="PropertyValue">
        /// The property value.
        /// </param>
        public void AddPropertyData(string PropertyAlias, int PropertyValue)
        {
            var matchingPropData = this.PropertyData.Where(p => p.PropertyAlias == PropertyAlias).FirstOrDefault();

            if (matchingPropData != null)
            {
                //update existing
                matchingPropData.SetValue(PropertyValue);
            }
            else
            {
                //Add new
                var NewProp = GetOrCreatePropertyData(PropertyAlias);
                NewProp.SetValue(PropertyValue);
                this.PropertyData.Add(NewProp);
            }
        }

        /// <summary>
        /// Add DATETIME property data to this location
        /// </summary>
        /// <param name="PropertyAlias">
        /// The property alias.
        /// </param>
        /// <param name="PropertyValue">
        /// The property value.
        /// </param>
        public void AddPropertyData(string PropertyAlias, DateTime PropertyValue)
        {
            var matchingPropData = this.PropertyData.Where(p => p.PropertyAlias == PropertyAlias).FirstOrDefault();

            if (matchingPropData != null)
            {
                //update existing
                matchingPropData.SetValue(PropertyValue);
            }
            else
            {
                //Add new
                var NewProp = GetOrCreatePropertyData(PropertyAlias);
                NewProp.SetValue(PropertyValue);
                this.PropertyData.Add(NewProp);
            }
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
            var matchingPropData = this.PropertyData.Where(p => p.PropertyAlias == PropertyAlias).FirstOrDefault();

            if (matchingPropData != null)
            {
                //Update Prop
                matchingPropData.SetValue(PropertyValue);
            }
            else
            {
                //Add new prop
                var NewPropData = GetOrCreatePropertyData(PropertyAlias);
                NewPropData.SetValue(PropertyValue);
                this.PropertyData.Add(NewPropData);
            }
        }

        public void SyncPropertiesWithType()
        {
            var allLocTypeProps = this.GetLocTypeProperties();

            foreach (var prop in allLocTypeProps)
            {
                var NewPropData = this.GetOrCreatePropertyData(prop.Alias);
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
            //this.LocationType = Repositories.LocationTypeRepo.GetByKey(LocTypeKey);
            this.PropertyData = this.GetPropertyData().ToList();
        }

        private IEnumerable<LocationPropertyData> GetPropertyData()
        {
            var finalPropData = new List<LocationPropertyData>();

            //Get list of properties which should be represented
            var propsList = GetLocTypeProperties();

            //Get current properties
            var currentPropData = Repositories.LocationPropertyDataRepo.GetByLocation(this.Key);

            if (!currentPropData.Any())
            {
                //no data, so add all loctype props
                foreach (var locTypeProp in propsList)
                {
                    var newProp = new LocationPropertyData(this.Key, locTypeProp.Key);
                    finalPropData.Add(newProp);
                }
            }
            else
            {
                //compare lists of props
                foreach (var locTypeProp in propsList)
                {
                    var prop = currentPropData.Where(p => p.LocationTypePropertyKey == locTypeProp.Key).FirstOrDefault();
                    if (prop != null)
                    {
                        finalPropData.Add(prop);
                    }
                    else
                    {
                        var newProp = new LocationPropertyData(this.Key, locTypeProp.Key);
                        finalPropData.Add(newProp);
                    }
                }
            }

            return finalPropData;
        }

        //private IEnumerable<LocationTypeProperty> GetLocTypeProperties()
        //{
        //    return GetLocTypeProperties(Constants.DefaultLocationTypeKey);
        //}

        private IEnumerable<LocationTypeProperty> GetLocTypeProperties()
        {
            //Guid LocTypeKey
            var propsList = new List<LocationTypeProperty>();

            //add default props
            var defaultProps = Repositories.LocationTypePropertyRepo.GetByLocationType(Constants.DefaultLocationTypeKey);

            propsList.AddRange(defaultProps);

            //add any custom type props
            if (this.LocationTypeKey != Constants.DefaultLocationTypeKey)
            {
                var customProps = Repositories.LocationTypePropertyRepo.GetByLocationType(this.LocationTypeKey);
                propsList.AddRange(customProps);
            }

            return propsList;
        }

        private LocationPropertyData GetOrCreatePropertyData(string PropertyAlias)
        {
            //First, Lookup property type information
            var locTypeProp = Repositories.LocationTypePropertyRepo.GetByAlias(PropertyAlias);

            if (locTypeProp == null)
            {
                throw new Exception("Provided property alias does not match a valid property for this location type.");
            }
            else
            {
                //Now check for existing prop data
                Guid LocationKey = this.Key;
                var existingPropertyData = Repositories.LocationPropertyDataRepo.GetByAlias(PropertyAlias, LocationKey);

                if (existingPropertyData != null)
                {
                    return existingPropertyData;
                }
                else
                {
                    var newPropData = new LocationPropertyData(this.Key, locTypeProp.Key);
                    Repositories.LocationPropertyDataRepo.Insert(newPropData);
                    return newPropData;
                }
            }
        }

        #endregion



    }

}