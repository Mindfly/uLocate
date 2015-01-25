namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Helpers;

    using uLocate.Persistance;

    public class JsonLocation
    {
        public Guid Key { get; set; }
        public Guid LocationTypeKey { get; set; }
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
        public List<JsonPropertyData> PropertyData { get; set; }

        public JsonLocation()
        {
            this.PropertyData = new List<JsonPropertyData>();
        }

        public JsonLocation(Location ConvertedFromLocation)
        {
            //Basic Location properties 
            this.Key = ConvertedFromLocation.Key;
            this.Name = ConvertedFromLocation.Name;
            this.LocationTypeKey = ConvertedFromLocation.LocationTypeKey;

            this.Latitude = ConvertedFromLocation.Latitude;
            this.Longitude = ConvertedFromLocation.Longitude;

            //Address
            this.Address1 = ConvertedFromLocation.Address.Address1;
            this.Address2 = ConvertedFromLocation.Address.Address2;
            this.Locality = ConvertedFromLocation.Address.Locality;
            this.Region = ConvertedFromLocation.Address.Region;
            this.PostalCode = ConvertedFromLocation.Address.PostalCode;
            this.CountryCode = ConvertedFromLocation.Address.CountryCode;

            this.PropertyData = new List<JsonPropertyData>();
            foreach (var Prop in ConvertedFromLocation.PropertyData)
            {
                //Check for special props
                switch (Prop.PropertyAlias)
                {
                    case "Phone":
                        this.Phone = Prop.dataNvarchar;
                        break;
                    case "Email":
                        this.Email = Prop.dataNvarchar;
                        break;
                    default:
                        var JsonProp = new JsonPropertyData();
                        JsonProp.Key = Prop.Key;
                        JsonProp.PropAlias = Prop.PropertyAlias;
                        JsonProp.PropData = Prop.Value;
                        this.PropertyData.Add(JsonProp);
                        break;
                }
             }
        }

        public Location ConvertToLocation()
        {
            Location Entity;

            if (this.Key != Guid.Empty)
            {
                //Lookup existing entity
                Entity = Repositories.LocationRepo.GetByKey(this.Key);

                //Update Location properties as needed
                Entity.Name = this.Name;
                Entity.LocationTypeKey = this.LocationTypeKey;

                Entity.Latitude = this.Latitude;
                Entity.Longitude = this.Longitude;

                //Deal with Address
                if (Entity.Address != null)
                {
                    Entity.Address.Address1 = this.Address1;
                    Entity.Address.Address2 = this.Address2;
                    Entity.Address.Locality = this.Locality;
                    Entity.Address.Region = this.Region;
                    Entity.Address.PostalCode = this.PostalCode;
                    Entity.Address.CountryCode = this.CountryCode;
                }
                else
                {
                    //Add Address properties
                    Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Address1, this.Address1);
                    Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Address2, this.Address2);
                    Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Locality, this.Locality);
                    Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Region, this.Region);
                    Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.PostalCode, this.PostalCode);
                    Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.CountryCode, this.CountryCode);
                }

                //Deal with Properties    
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Phone, this.Phone);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Email, this.Email);

//                Entity.PropertyData = "x";
  //              Entity.

                //match up properties
                //foreach (var JsonProp in this.Properties)
                //{
                //    //lookup existing property
                //    var Prop = Entity.Properties.Where(p => p.Key == JsonProp.Key).FirstOrDefault();

                //    if (Prop != null)
                //    {
                //        Prop.Alias = JsonProp.PropAlias;
                //        Prop.Name = JsonProp.PropName;
                //        Prop.DataTypeId = JsonProp.PropType;
                //        //Repositories.LocationTypePropertyRepo.Update(Prop);
                //    }
                //    else
                //    {
                //        //Add new property
                //        Entity.AddProperty(JsonProp.PropAlias, JsonProp.PropName, JsonProp.PropType);
                //    }
                //}
            }
            else
            {
                //Create new entity
                Entity = new Location()
                {
                    Name = this.Name,
                    LocationTypeKey = this.LocationTypeKey,

                    Latitude = this.Latitude,
                    Longitude = this.Longitude,
                };

                //Add Default properties
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Address1, this.Address1);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Address2, this.Address2);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Locality, this.Locality);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Region, this.Region);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.PostalCode, this.PostalCode);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.CountryCode, this.CountryCode);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Phone, this.Phone);
                Entity.AddPropertyData(Constants.DefaultLocPropertyAlias.Email, this.Email);
           
        
                //Add properties
                foreach (var JsonProp in this.PropertyData)
                {
                   Entity.AddPropertyData(JsonProp.PropAlias, JsonProp.PropData);
                }
            }

            return Entity;
        }

    }

    public class JsonPropertyData
    {
        public Guid Key { get; set; }
        public string PropAlias { get; set; }
        public object PropData { get; set; }

    }
}
