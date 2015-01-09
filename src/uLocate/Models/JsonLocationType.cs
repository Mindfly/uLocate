namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Helpers;

    using uLocate.Persistance;

    public class JsonLocationType
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public List<JsonTypeProperty> Properties { get; set; }

        public JsonLocationType()
        {
            this.Properties = new List<JsonTypeProperty>();
        }

        public LocationType ConvertToLocationType()
        {
            LocationType Entity;

            if (this.Key != Guid.Empty)
            {
                //Lookup existing entity
                Entity = Repositories.LocationTypeRepo.GetByKey(this.Key);

                //Update LT properties as needed
                Entity.Name = this.Name;
                Entity.Description = this.Description;
                Entity.Icon = this.Icon;

                //match up properties
                foreach (var JsonProp in this.Properties)
                {
                    //lookup existing property
                    var Prop = Entity.Properties.Where(p => p.Key == JsonProp.Key).FirstOrDefault();

                    if (Prop != null)
                    {
                        Prop.Alias = JsonProp.PropAlias;
                        Prop.Name = JsonProp.PropName;
                        Prop.DataTypeId = JsonProp.PropType;
                        //Repositories.LocationTypePropertyRepo.Update(Prop);
                    }
                    else
                    {
                        //Add new property
                        Entity.AddProperty(JsonProp.PropAlias, JsonProp.PropName, JsonProp.PropType);
                    }
                }
            }
            else
            {
                //Create new entity
                Entity = new LocationType()
                {
                    Name = this.Name,
                    Description = this.Description,
                    Icon = this.Icon,
                };

                //Add properties
                foreach (var JsonProp in this.Properties)
                {
                    Entity.AddProperty(JsonProp.PropAlias, JsonProp.PropName, JsonProp.PropType);
                }
            }

            return Entity;
        }

    }

    public class JsonTypeProperty
    {
        public Guid Key { get; set; }
        public string PropName { get; set; }
        public string PropAlias { get; set; }
        public int PropType { get; set; }

    }
}
