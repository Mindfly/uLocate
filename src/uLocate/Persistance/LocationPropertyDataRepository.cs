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
    internal class LocationPropertyDataRepository : RepositoryBase<LocationPropertyData> 
    {
        /// <summary>
        /// The current collection of Location Types
        /// </summary>
        private List<LocationPropertyData> CurrentCollection = new List<LocationPropertyData>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationPropertyDataRepository"/> class. 
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public LocationPropertyDataRepository(UmbracoDatabase database, IRuntimeCacheProvider cache)
            : base(database, cache)
        {
           // this._database = database;
        }

        #region Public Methods

        public void Insert(LocationPropertyData Entity)
        {
            PersistNewItem(Entity);
        }

        //public void Insert(LocationPropertyData Entity, out Guid NewItemKey)
        //{
        //    //TODO: might not be needed
        //    PersistNewItem(Entity);
        //    NewItemKey = Entity.Key;
        //}

        public void Delete(Guid PropertyKey)
        {
            LocationPropertyData ThisProperty = this.GetByKey(PropertyKey);
            this.Delete(ThisProperty);
        }

        public StatusMessage Delete(LocationPropertyData Entity)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            PersistDeletedItem(Entity, out ReturnMsg);

            return ReturnMsg; 
        }

        public void Update(LocationPropertyData Entity)
        {
            PersistUpdatedItem(Entity);
        }

        public LocationPropertyData GetByKey(Guid Key)
        {
            CurrentCollection.Clear();
            CurrentCollection.Add(Get(Key));
            FillChildren();

            return CurrentCollection[0]; 
        }

        public IEnumerable<LocationPropertyData> GetByKey(Guid[] Keys)
        {
            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(Keys));
            FillChildren();

            return CurrentCollection; 
        }

        public LocationPropertyData GetByAlias(string PropertyAlias, Guid LocationKey)
        {
            CurrentCollection.Clear();
            var sql = new Sql();
            sql
                .Select("*")
                .From<LocationPropertyDataDto>()
                .InnerJoin<LocationTypePropertyDto>()
                .On<LocationPropertyDataDto, LocationTypePropertyDto>(left => left.LocationTypePropertyKey, right => right.Key)
                .Where<LocationTypePropertyDto>(n => n.Alias == PropertyAlias)
                .Where<LocationPropertyDataDto>(n => n.LocationKey == LocationKey);

            var dtoResult = Repositories.ThisDb.Fetch<LocationPropertyDataDto>(sql).FirstOrDefault();

            if (dtoResult == null)
                return null;

            var converter = new DtoConverter();
            var entity = converter.ToLocationPropertyDataEntity(dtoResult);

            return entity;
            FillChildren();

            return CurrentCollection[0];
        }

        public IEnumerable<LocationPropertyData> GetAll()
        {
            var EmptyParams = new Guid[] { };

            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(EmptyParams));
            FillChildren();

            return CurrentCollection; 
        }

        //public IEnumerable<LocationPropertyData> GetByLocationType(int LocationTypeId)
        //{
        //    CurrentCollection.Clear();
        //    var MySql = new Sql();
        //    MySql.Select("*").From<LocationPropertyData>().Where("LocationTypeId = @0", LocationTypeId);
        //    CurrentCollection.AddRange(Repositories.ThisDb.Fetch<LocationPropertyData>(MySql).ToList());
        //    FillChildren();

        //    return CurrentCollection; 
        //}

        public IEnumerable<LocationPropertyData> GetByLocation(Guid LocationKey)
        {
            List<LocationPropertyData> Result = new List<LocationPropertyData>();
            IEnumerable<LocationPropertyDataDto> dtoResults;

            CurrentCollection.Clear();

            var sql = new Sql();
            sql.Select("*").From<LocationPropertyDataDto>().Where("LocationKey = @0", LocationKey);

            dtoResults = Repositories.ThisDb.Fetch<LocationPropertyDataDto>(sql).ToList();

            var converter = new DtoConverter();
            foreach (var result in dtoResults)
            {
                Result.Add(converter.ToLocationPropertyDataEntity(result));
            }

            CurrentCollection.AddRange(Result);
            FillChildren();

            return CurrentCollection; 
        }

        #endregion

        #region Protected Methods

        protected override IEnumerable<LocationPropertyData> PerformGetAll(params Guid[] Keys)
        {
            List<LocationPropertyData> Result = new List<LocationPropertyData>();
            IEnumerable<LocationPropertyDataDto> dtoResults;

            if (Keys.Any())
            {
                foreach (var key in Keys)
                {
                    Result.Add(Get(key));
                }
            }
            else
            {
                var sql = new Sql();
                sql.Select("*").From<LocationPropertyDataDto>();

                dtoResults = Repositories.ThisDb.Fetch<LocationPropertyDataDto>(sql).ToList();

                var converter = new DtoConverter();
                foreach (var result in dtoResults)
                {
                    Result.Add(converter.ToLocationPropertyDataEntity(result));
                }
            }

            return Result;
        }

        protected override LocationPropertyData PerformGet(Guid Key)
        {
            var sql = new Sql();
            sql
                .Select("*")
                .From<LocationPropertyDataDto>()
                .Where<LocationPropertyDataDto>(n => n.Key == Key);

            var dtoResult = Repositories.ThisDb.Fetch<LocationPropertyDataDto>(sql).FirstOrDefault();

            if (dtoResult == null)
                return null;

            var converter = new DtoConverter();
            var entity = converter.ToLocationPropertyDataEntity(dtoResult);

            return entity;
        }

        protected override void PersistNewItem(LocationPropertyData item)
        {
            string Msg = string.Format("LocationPropertyData '{0}' has been saved.", item.Value.ToString());

            item.AddingEntity();

            var converter = new DtoConverter();
            var dto = converter.ToLocationPropertyDataDto(item);

            Repositories.ThisDb.Insert(dto);
            //item.Key = dto.Key;

            //LogHelper.Info(typeof(LocationPropertyDataRepository), Msg);
        }

        protected override void PersistUpdatedItem(LocationPropertyData item)
        {
            string Msg = string.Format("LocationPropertyData '{0}' has been updated.", item.Value.ToString());
            item.UpdatingEntity();

            var converter = new DtoConverter();
            var dto = converter.ToLocationPropertyDataDto(item);

            Repositories.ThisDb.Update(dto);
            LogHelper.Info(typeof(LocationPropertyDataRepository), Msg);
        }

        protected override void PersistDeletedItem(LocationPropertyData item, out StatusMessage StatusMsg)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = string.Concat(item.PropertyAttributes.Name, "=",item.Value.ToString());

            ReturnMsg.Message = string.Format("LocationPropertyData '{0}' has been deleted.", ReturnMsg.ObjectName);
            var converter = new DtoConverter();
            var dto = converter.ToLocationPropertyDataDto(item);

            Repositories.ThisDb.Delete(dto);
            ReturnMsg.Success = true;

            StatusMsg = ReturnMsg;
            //LogHelper.Info(typeof(LocationPropertyDataRepository), ReturnMsg.Message);
        }

        protected override Sql GetBaseQuery(bool isCount)
        {
            var MySql = new Sql();
            MySql.Select("*").From<LocationPropertyDataDto>();
            return MySql;
        }

        protected override string GetBaseWhereClause()
        {
            return " WHERE Key= @0";
        }

        protected override IEnumerable<string> GetDeleteClauses()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void FillChildren()
        {
            //this.FillDataTypeInfo();
        }

        //private void FillDataTypeInfo()
        //{
        //    foreach (var Prop in CurrentCollection)
        //    {
        //        var PropDtId = Prop.PropertyAttributes.DataTypeId;
        //        var MySql = new Sql();
        //        MySql
        //            .Select("*")
        //            .From<cmsDataTypeDto>()
        //            .Where<cmsDataTypeDto>(n => n.DataTypeId == PropDtId);

        //        var MatchingDt = Repositories.ThisDb.Fetch<cmsDataTypeDto>(MySql).FirstOrDefault();
        //        if (MatchingDt != null)
        //        {
        //            Prop.PropertyAttributes.DatabaseType = MatchingDt.DatabaseType;
        //            Prop.PropertyAttributes.PropertyEditorAlias = MatchingDt.PropertyEditorAlias;
        //        }
        //    }
        //}

        #endregion


    }
}
