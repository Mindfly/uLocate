namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;

    using uLocate.Persistance;

    /// <summary>
    /// Represents a read-only Location object - generally pulled from the Examine Index
    /// Note: This was previously named "JsonLocation" and was changed 2015-07-10 to "IndexedLocation"
    /// </summary>
    public class IndexedLocation
    {
        public Guid Key { get; set; }
        public Guid LocationTypeKey { get; set; }
        public string LocationTypeName { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<IndexedPropertyData> CustomPropertyData { get; set; }
        public Dictionary<string,string> AllPropertiesDictionary { get; set; }

        public IndexedLocation()
        {
            this.CustomPropertyData = new List<IndexedPropertyData>();
        }

        public IndexedLocation(EditableLocation ConvertedFromEditableLocation)
        {
            //Basic Location properties 
            this.Key = ConvertedFromEditableLocation.Key;
            this.Name = ConvertedFromEditableLocation.Name;
            this.LocationTypeKey = ConvertedFromEditableLocation.LocationTypeKey;
            this.LocationTypeName = ConvertedFromEditableLocation.LocationType.Name;

            this.Latitude = ConvertedFromEditableLocation.Latitude;
            this.Longitude = ConvertedFromEditableLocation.Longitude;

            //Address
            this.Address1 = ConvertedFromEditableLocation.Address.Address1;
            this.Address2 = ConvertedFromEditableLocation.Address.Address2;
            this.Locality = ConvertedFromEditableLocation.Address.Locality;
            this.Region = ConvertedFromEditableLocation.Address.Region;
            this.PostalCode = ConvertedFromEditableLocation.Address.PostalCode;
            this.CountryCode = ConvertedFromEditableLocation.Address.CountryCode;

            this.CustomPropertyData = new List<IndexedPropertyData>();
            foreach (var Prop in ConvertedFromEditableLocation.PropertyData)
            {
                //Check for special props
                switch (Prop.PropertyAlias)
                {
                    case Constants.DefaultLocPropertyAlias.Phone:
                        this.Phone = Prop.Value.ToString();
                        break;
                    case Constants.DefaultLocPropertyAlias.Email:
                        this.Email = Prop.Value.ToString();
                        break;
                    case Constants.DefaultLocPropertyAlias.Address1:
                        break;
                    case Constants.DefaultLocPropertyAlias.Address2:
                        break;
                    case Constants.DefaultLocPropertyAlias.Locality:
                        break;
                    case Constants.DefaultLocPropertyAlias.Region:
                        break;
                    case Constants.DefaultLocPropertyAlias.PostalCode:
                        break;
                    case Constants.DefaultLocPropertyAlias.CountryCode:
                        break;
                    default:
                        this.CustomPropertyData.Add(new IndexedPropertyData(Prop));
                        //this.AllPropertyData.Add(new IndexedPropertyData(Prop));
                        break;
                }
            }
        }

        public EditableLocation ConvertToLocation()
        {
            EditableLocation Entity;

            if (this.Key != Guid.Empty)
            {
                //Lookup existing entity
                Entity = Repositories.LocationRepo.GetByKey(this.Key);

                //Update Location properties as needed
                Entity.Name = this.Name;
                Entity.LocationTypeKey = this.LocationTypeKey;

            }
            else
            {
                //Create new entity
                Entity = new EditableLocation(Name = this.Name, LocationTypeKey = this.LocationTypeKey);
            }

            //Update lat/long
            Entity.Latitude = this.Latitude;
            Entity.Longitude = this.Longitude;

            //Add Address properties
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Address1, this.Address1);
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Address2, this.Address2);
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Locality, this.Locality);
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Region, this.Region);
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.PostalCode, this.PostalCode);
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.CountryCode, this.CountryCode);

            //Deal with Properties    
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Phone, this.Phone);
            Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Email, this.Email);

            //Add custom properties
            foreach (var JsonProp in this.CustomPropertyData)
            {
                Entity.AddPropertyData(JsonProp.PropAlias, JsonProp.PropData);
            }

            return Entity;
        }

    }

    public class IndexedPropertyData
    {
        public Guid Key { get; set; }
        public string PropAlias { get; set; }
        public object PropData { get; set; }

        internal IndexedPropertyData(LocationPropertyData Prop)
        {
            if (Prop != null)
            {
                this.Key = Prop.Key;
                this.PropAlias = Prop.PropertyAlias;
                this.PropData = Prop.Value.ValueObject;
            }
        }

        internal IndexedPropertyData()
        {
        }
    }
}
