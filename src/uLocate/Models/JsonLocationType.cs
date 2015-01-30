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

        public JsonLocationType(LocationType ConvertedFromLocationType)
        {
            this.Key = ConvertedFromLocationType.Key;
            this.Name = ConvertedFromLocationType.Name;
            this.Description = ConvertedFromLocationType.Description;
            this.Icon = ConvertedFromLocationType.Icon;
            this.Properties = new List<JsonTypeProperty>();
            foreach (var Prop in ConvertedFromLocationType.Properties)
            {
                var JsonProp = new JsonTypeProperty();

                JsonProp.PropAlias = Prop.Alias;
                JsonProp.PropName = Prop.Name;
                JsonProp.PropType = Prop.DataTypeId;
                JsonProp.IsDefaultProp = Prop.IsDefaultProp;
                JsonProp.Key = Prop.Key;
                this.Properties.Add(JsonProp);
            }
        }

        public LocationType ConvertToLocationType()
        {
            LocationType Entity;

            if (this.Key != Guid.Empty)
            {
                //Lookup existing entity
                Entity = Repositories.LocationTypeRepo.GetByKey(this.Key);

                if (Entity == null)
                {
                    Entity = this.CreateNew();
                }
            }
            else
            {
                Entity = this.CreateNew();
            }

            //Update LT properties as needed
            Entity.Name = this.Name;
            Entity.Description = this.Description;
            Entity.Icon = this.Icon;

            //match up child properties
            foreach (var JsonProp in this.Properties)
            {
                //lookup existing property
                var Prop = Entity.Properties.Where(p => p.Key == JsonProp.Key).FirstOrDefault();

                if (Prop != null)
                {
                    Prop.Alias = JsonProp.PropAlias;
                    Prop.Name = JsonProp.PropName;
                    Prop.DataTypeId = JsonProp.PropType;
                    Repositories.LocationTypePropertyRepo.Update(Prop);
                }
                else
                {
                    //Add new property
                    Entity.AddProperty(JsonProp.PropAlias, JsonProp.PropName, JsonProp.PropType);
                }
            }

            return Entity;
        }

        private LocationType CreateNew()
        {
            //Create new entity
            var Entity = new LocationType()
            {
                Key = this.Key,
                Name = this.Name,
                Description = this.Description,
                Icon = this.Icon,
            };

            return Entity;
        }
    }

    public class JsonTypeProperty
    {
        public Guid Key { get; set; }
        public string PropName { get; set; }
        public string PropAlias { get; set; }
        public int PropType { get; set; }
        public Boolean IsDefaultProp { get; set; }

        public LocationTypeProperty ConvertToLocationTypeProperty()
        {
            LocationTypeProperty Entity;

            if (this.Key != Guid.Empty)
            {
                //Lookup existing entity
                Entity = Repositories.LocationTypePropertyRepo.GetByKey(this.Key);

                if (Entity == null)
                {
                    Entity = this.CreateNew();
                }
            }
            else
            {
                Entity = this.CreateNew();
            }

            //Update
            Entity.Alias = this.PropAlias;
            Entity.Name = this.PropName;
            Entity.DataTypeId = this.PropType;

            return Entity;
        }

        private LocationTypeProperty CreateNew()
        {
            //Create new entity
            var Entity = new LocationTypeProperty()
            {
                Key = this.Key,
                Alias = this.PropAlias,
                Name = this.PropName,
                DataTypeId = this.PropType
            };
            return Entity;
        }
    }
}
