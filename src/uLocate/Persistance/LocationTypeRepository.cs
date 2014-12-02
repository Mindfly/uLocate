namespace uLocate.Persistance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Data;
    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
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
            PersistNewItem(Entity);
        }

        public void Delete(LocationType Entity)
        {
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
            CurrentCollection.Clear();
            IEnumerable<LocationType> Result;
            var MySql = new Sql();

            if (IdKeys.Any())
            {
                var ParamsCsv = string.Join(",", IdKeys);
                MySql.Select("*").From<LocationTypeDto>().Where("Id IN @0;", ParamsCsv);
            }
            else
            {
                MySql.Select("*").From<LocationTypeDto>();
            }

            Result = Repositories.ThisDb.Fetch<LocationType>(MySql).ToList();

            return Result;
        }

        protected override LocationType PerformGet(object IdKey)
        {
            var MySql = new Sql();
            MySql.Select("*").From<LocationTypeDto>().Where("Id = @0;", IdKey);

            return Repositories.ThisDb.Fetch<LocationType>(MySql).FirstOrDefault();
        }

        protected override void PersistNewItem(LocationType item)
        {
            throw new NotImplementedException();
        }

        protected override void PersistUpdatedItem(LocationType item)
        {
            throw new NotImplementedException();
        }

        protected override void PersistDeletedItem(LocationType item)
        {
            //var MySql = new Sql();
            //MySql.Select("*").From<LocationTypeDto>();
            throw new NotImplementedException();
        }

        protected override Sql GetBaseQuery(bool isCount)
        {
            var MySql = new Sql();
            MySql.Select("*").From<LocationTypeDto>();
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

        private void FillProperties()
        {
            var Repo = new LocationTypePropertyRepository(Repositories.ThisDb, Helper.ThisCache);
            foreach (var LocType in CurrentCollection)
            {
                LocType.Properties = Repo.GetByLocationType(LocType.Id);
            }
        }

        #endregion
    }
}