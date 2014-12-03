using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Persistance
{
    using uLocate.Data;
    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Repository for working with LocationTypeProperties
    /// </summary>
    internal class LocationTypePropertyRepository : RepositoryBase<LocationTypeProperty> //, ILocationTypePropertyRepository
    {
        /// <summary>
        /// The current collection of Location Types
        /// </summary>
        private List<LocationTypeProperty> CurrentCollection = new List<LocationTypeProperty>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationTypePropertyRepository"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public LocationTypePropertyRepository(UmbracoDatabase database, IRuntimeCacheProvider cache)
            : base(database, cache)
        {
           // this._database = database;
        }

        #region Public Methods

        public void Insert(LocationTypeProperty Entity)
        {
            var NewItemId = PersistNewItem(Entity);
        }

        public void Insert(LocationTypeProperty Entity, out int NewItemId)
        {
            var NewItemInfo = PersistNewItem(Entity);
            NewItemId = Convert.ToInt32(NewItemInfo);
        }

        public void Delete(int PropertyId)
        {
            LocationTypeProperty ThisProperty = this.GetById(PropertyId);
            this.Delete(ThisProperty);
        }

        public void Delete(LocationTypeProperty Entity)
        {
            PersistDeletedItem(Entity);
        }

        public void Update(LocationTypeProperty Entity)
        {
            PersistUpdatedItem(Entity);
        }

        public LocationTypeProperty GetById(int Id)
        {
            CurrentCollection.Clear();
            CurrentCollection.Add(Get(Id));
            FillChildren();

            return CurrentCollection[0]; 
        }

        public IEnumerable<LocationTypeProperty> GetById(int[] Ids)
        {
            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(Ids));
            FillChildren();

            return CurrentCollection; 
        }

        public IEnumerable<LocationTypeProperty> GetAll()
        {
            var EmptyParams = new object[] { };

            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(EmptyParams));
            FillChildren();

            return CurrentCollection; 
        }

        public IEnumerable<LocationTypeProperty> GetByLocationType(int LocationTypeId)
        {
            CurrentCollection.Clear();
            var MySql = new Sql();
            MySql.Select("*").From<LocationTypeProperty>().Where("LocationTypeId = @0", LocationTypeId);
            CurrentCollection.AddRange(Repositories.ThisDb.Fetch<LocationTypeProperty>(MySql).ToList());
            FillChildren();

            return CurrentCollection; 
        }

        #endregion

        #region Protected Methods

        protected override IEnumerable<LocationTypeProperty> PerformGetAll(params object[] IdKeys)
        {
            var MySql = new Sql();

            if (IdKeys.Any())
            {
                var ParamsCsv = string.Join(",", IdKeys);
                MySql.Select("*").From<LocationTypeProperty>().Where("Id IN @0", ParamsCsv);
            }
            else
            {
                MySql.Select("*").From<LocationTypeProperty>();
            }

            return Repositories.ThisDb.Fetch<LocationTypeProperty>(MySql).ToList();
        }

        protected override LocationTypeProperty PerformGet(object IdKey)
        {
            var MySql = new Sql();
            MySql.Select("*").From<LocationTypeProperty>().Where("Id = @0", IdKey);

            return Repositories.ThisDb.Fetch<LocationTypeProperty>(MySql).FirstOrDefault();
        }

        protected override object PersistNewItem(LocationTypeProperty item)
        {
            string Msg = string.Format("LocationTypeProperty '{0}' has been saved.", item.Name);
            var InsertedItem = Repositories.ThisDb.Insert(item);
            LogHelper.Info(typeof(LocationTypePropertyRepository), Msg);

            return InsertedItem;
        }

        protected override void PersistUpdatedItem(LocationTypeProperty item)
        {
            string Msg = string.Format("LocationTypeProperty '{0}' has been updated.", item.Name);
            Repositories.ThisDb.Update(item);
            LogHelper.Info(typeof(LocationTypePropertyRepository), Msg);
        }

        protected override void PersistDeletedItem(LocationTypeProperty item)
        {
            string Msg = string.Format("LocationTypeProperty '{0}' has been deleted.", item.Name);
            Repositories.ThisDb.Delete<LocationTypeProperty>(item.Id);
            LogHelper.Info(typeof(LocationTypePropertyRepository), Msg);
        }

        protected override Sql GetBaseQuery(bool isCount)
        {
            var MySql = new Sql();
            MySql.Select("*").From<LocationTypeProperty>();
            return MySql;
        }

        protected override string GetBaseWhereClause()
        {
            return " WHERE Id= @0";
        }

        protected override IEnumerable<string> GetDeleteClauses()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void FillChildren()
        {
            this.FillDataTypeInfo();
        }

        private void FillDataTypeInfo()
        {
            foreach (var Prop in CurrentCollection)
            {
                var PropDtId = Prop.DataTypeId;
                var MySql = new Sql();
                MySql
                    .Select("*")
                    .From<cmsDataTypeDto>()
                    .Where<cmsDataTypeDto>(n => n.DataTypeId == PropDtId);

                var MatchingDt = Repositories.ThisDb.Fetch<cmsDataTypeDto>(MySql).FirstOrDefault();
                if (MatchingDt != null)
                {
                    Prop.DatabaseType = MatchingDt.DatabaseType;
                    Prop.PropertyEditorAlias = MatchingDt.PropertyEditorAlias;
                }
            }
        }

        #endregion
    }
}
