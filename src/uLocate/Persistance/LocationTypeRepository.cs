namespace uLocate.Persistance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Data;
    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Represents the <see cref="LocationTypeRepository"/>.
    /// </summary>
    internal class LocationTypeRepository : RepositoryBase<LocationType> //, ILocationTypeRepository
    {
        /// <summary>
        /// The database.
        /// </summary>

        private List<LocationType> CurrentCollection = new List<LocationType>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationTypeRepository"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public LocationTypeRepository(UmbracoDatabase database, IRuntimeCacheProvider cache) : base(database, cache)
        {
        }

        #region Public Methods
        public void Insert(LocationType Entity)
        {
            var NewItemId = PersistNewItem(Entity);
        }

        public void Insert(LocationType Entity, out int NewItemId)
        {
            var NewItemInfo = PersistNewItem(Entity);
            NewItemId = Convert.ToInt32(NewItemInfo);
        }

        public void Delete(int LocationTypeId, bool ConvertLocationsToDefault = true)
        {
            LocationType ThisLocType = this.GetById(LocationTypeId);
            if (ThisLocType != null)
            {
                this.Delete(ThisLocType, ConvertLocationsToDefault);
            }
        }

        public void Delete(LocationType Entity, bool ConvertLocationsToDefault = true)
        {
            if (ConvertLocationsToDefault)
            {
                //TODO: Add Location handling logic here
                //Check for associated locations and update them to use LocationTypeId = DefaultId
                
            }
            else
            {
                //Cascade deletes to matching Locations
            }

            PersistDeletedItem(Entity);
        }
        
        public void Update(LocationType Entity)
        {
            PersistUpdatedItem(Entity);
        }

        public LocationType GetById(int Id)
        {
            CurrentCollection.Clear();
            CurrentCollection.Add((LocationType)Get(Id));
            FillChildren();

            return CurrentCollection[0]; 
        }

        public IEnumerable<LocationType> GetById(int[] Ids)
        {
            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(Ids));
            FillChildren();

            return CurrentCollection; 
        }

        public IEnumerable<LocationType> GetAll()
        {
            var EmptyParams = new object[] { };

            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(EmptyParams));
            FillChildren();

            return CurrentCollection; 
        }

        #endregion

        #region Protected Methods
        protected override IEnumerable<LocationType> PerformGetAll(params object[] IdKeys)
        {
            IEnumerable<LocationType> Result;
            var MySql = new Sql();

            if (IdKeys.Any())
            {
                var ParamsCsv = string.Join(",", IdKeys);
                MySql.Select("*").From<LocationType>().Where("Id IN @0;", ParamsCsv);
            }
            else
            {
                MySql.Select("*").From<LocationType>();
            }

            Result = Repositories.ThisDb.Fetch<LocationType>(MySql).ToList();

            return Result;
        }

        protected override LocationType PerformGet(object IdKey)
        {
            var MySql = new Sql();
            MySql
                .Select("*")
                .From<LocationType>()
                .Where("Id = @0", IdKey);
            //.Where<LocationType>(n => n.Id == IdKey);
            return Repositories.ThisDb.Fetch<LocationType>(MySql).FirstOrDefault();
        }

        protected override object PersistNewItem(LocationType item)
        {
            string Msg = string.Format("LocationType '{0}' has been saved.", item.Name);
            var InsertedItem = Repositories.ThisDb.Insert(item);
            LogHelper.Info(typeof(LocationTypeRepository), Msg);
            
            PersistChildren(item);

            return InsertedItem;
        }

        protected override void PersistUpdatedItem(LocationType item)
        {
            string Msg = string.Format("LocationType '{0}' has been updated.", item.Name);
            Repositories.ThisDb.Update(item);
            LogHelper.Info(typeof(LocationTypeRepository), Msg);
        }

        protected override void PersistDeletedItem(LocationType item)
        {
            DeleteChildren(item);
            string Msg = string.Format("LocationType '{0}' has been deleted.", item.Name);
            Repositories.ThisDb.Delete<LocationType>(item.Id);
            LogHelper.Info(typeof(LocationTypeRepository), Msg);
        }

        protected override Sql GetBaseQuery(bool isCount)
        {
            var MySql = new Sql();
            MySql.Select("*").From<LocationType>();
            return MySql;
        }

        protected override string GetBaseWhereClause()
        {
            return " WHERE Id= @0;";
        }

        protected override IEnumerable<string> GetDeleteClauses()
        {
            throw new NotImplementedException();
            //var Strings = new IEnumerable<string>;
            //Strings.a dd(" WHERE Id= {0};";)
            //return 
        }

        #endregion

        #region Private Methods

        private void FillChildren()
        {
            this.FillProperties();
        }

        private void PersistChildren(LocationType item)
        {
            this.PersistProperties(item);
        }

        private void DeleteChildren(LocationType LocationType)
        {
            this.DeleteProperties(LocationType);
        }

        private void FillProperties()
        {
            var Repo = new LocationTypePropertyRepository(Repositories.ThisDb, Helper.ThisCache);
            foreach (var LocType in CurrentCollection)
            {
                LocType.Properties = Repo.GetByLocationType(LocType.Id).ToList();
            }
        }

        private void PersistProperties(LocationType item)
        {
            var Repo = new LocationTypePropertyRepository(Repositories.ThisDb, Helper.ThisCache);
            foreach (var NewProp in item.Properties)
            {
                if (NewProp.LocationTypeId == 0)
                {
                    NewProp.LocationTypeId = item.Id;
                }

                Repo.Insert(NewProp);
            }
        }

        private void DeleteProperties(LocationType item)
        {
            var Repo = new LocationTypePropertyRepository(Repositories.ThisDb, Helper.ThisCache);
            var MatchingProps = Repo.GetByLocationType(item.Id);

            foreach (var Prop in MatchingProps)
            {
                Repo.Delete(Prop);
            }
        }

        #endregion
    }
}