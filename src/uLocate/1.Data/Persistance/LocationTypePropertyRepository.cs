namespace uLocate.Persistance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    using uLocate.Data;
    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Repository for working with LocationTypeProperties
    /// </summary>
    internal class LocationTypePropertyRepository : RepositoryBase<LocationTypeProperty>
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
        }

        #region Public Methods

        public void Insert(LocationTypeProperty Entity)
        {
            PersistNewItem(Entity);
        }

        public void Insert(LocationTypeProperty Entity, out Guid NewItemKey)
        {
            //TODO: might not be needed
            PersistNewItem(Entity);
            NewItemKey = Entity.Key;
        }

        public StatusMessage Delete(Guid PropertyKey)
        {
            StatusMessage ReturnMsg = new StatusMessage();

            LocationTypeProperty ThisProperty = this.GetByKey(PropertyKey);
            if (ThisProperty != null)
            {
                PersistDeletedItem(ThisProperty, out ReturnMsg);
            }
            else
            {
                ReturnMsg.Success = false;
                ReturnMsg.Code = "NotFound";
                ReturnMsg.ObjectName = PropertyKey.ToString();
                ReturnMsg.Message = string.Format("Location Type Property with key '{0}' was not found and can not be deleted.", ReturnMsg.ObjectName);

            }

            return ReturnMsg;
        }

        public StatusMessage Delete(LocationTypeProperty Entity)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            PersistDeletedItem(Entity, out ReturnMsg);

            return ReturnMsg;
        }

        public void Update(LocationTypeProperty Entity)
        {
            var StoredEntity = this.GetByKey(Entity.Key);
            if (StoredEntity != null)
            {
                PersistUpdatedItem(Entity);
            }
            else
            {
                this.PersistNewItem(Entity);
            }
        }

        public LocationTypeProperty GetByAlias(string PropertyAlias)
        {
            CurrentCollection.Clear();
            var sql = new Sql();
            sql.Select("*")
                .From<LocationTypePropertyDto>()
                .Where<LocationTypePropertyDto>(n => n.Alias == PropertyAlias);

            var dtoResultList = Repositories.ThisDb.Fetch<LocationTypePropertyDto>(sql);

            if (dtoResultList != null)
            {
                foreach (var dtoResult in dtoResultList)
                {
                    var converter = new DtoConverter();
                    var entity = converter.ToLocationTypePropertyEntity(dtoResult);

                    CurrentCollection.Add(entity);
                }
            }

            return CurrentCollection[0];
        }
   

        /// <summary>
        /// Get the property by Key.
        /// </summary>
        /// <param name="Key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="LocationTypeProperty"/>.
        /// </returns>
        public LocationTypeProperty GetByKey(Guid Key)
        {
            CurrentCollection.Clear();
            var found = (LocationTypeProperty)Get(Key);

            if (found != null)
            {
                CurrentCollection.Add(found);
                FillChildren();
                return CurrentCollection[0];
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<LocationTypeProperty> GetByKey(Guid[] Keys)
        {
            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(Keys));
            FillChildren();

            return CurrentCollection;
        }

        public IEnumerable<LocationTypeProperty> GetAll()
        {
            var EmptyParams = new Guid[] { };

            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(EmptyParams));
            FillChildren();

            return CurrentCollection;
        }

        public IEnumerable<LocationTypeProperty> GetByLocationType(Guid LocationTypeKey)
        {
            CurrentCollection.Clear();
            var sql = new Sql();
            sql.Select("*")
                .From<LocationTypePropertyDto>()
                .Where<LocationTypePropertyDto>(n => n.LocationTypeKey == LocationTypeKey);

            var dtoResultList = Repositories.ThisDb.Fetch<LocationTypePropertyDto>(sql);

            if (dtoResultList != null)
            {
                foreach (var dtoResult in dtoResultList)
                {
                    var converter = new DtoConverter();
                    var entity = converter.ToLocationTypePropertyEntity(dtoResult);

                    CurrentCollection.Add(entity);
                    FillChildren();
                }
            }

            return CurrentCollection;
        }

        #endregion

        #region Protected Methods

        protected override IEnumerable<LocationTypeProperty> PerformGetAll(params Guid[] Keys)
        {
            List<LocationTypeProperty> Result = new List<LocationTypeProperty>();
            IEnumerable<LocationTypePropertyDto> dtoResults;

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
                sql.Select("*")
                    .From<LocationTypePropertyDto>();

                dtoResults = Repositories.ThisDb.Fetch<LocationTypePropertyDto>(sql).ToList();

                var converter = new DtoConverter();
                foreach (var result in dtoResults)
                {
                    Result.Add(converter.ToLocationTypePropertyEntity(result));
                }
            }

            return Result;
        }

        protected override LocationTypeProperty PerformGet(Guid Key)
        {
            var sql = new Sql();
            sql
                .Select("*")
                .From<LocationTypePropertyDto>()
                .Where<LocationTypePropertyDto>(n => n.Key == Key);

            var dtoResult = Repositories.ThisDb.Fetch<LocationTypePropertyDto>(sql).FirstOrDefault();

            if (dtoResult == null)
                return null;

            var converter = new DtoConverter();
            var entity = converter.ToLocationTypePropertyEntity(dtoResult);

            return entity;
        }

        protected override void PersistNewItem(LocationTypeProperty item)
        {
            string Msg = string.Format("LocationTypeProperty '{0}' has been saved.", item.Name);

            item.AddingEntity();

            var converter = new DtoConverter();
            var dto = converter.ToLocationTypePropertyDto(item);

            Repositories.ThisDb.Insert(dto);
            //item.Key = dto.Key;

            LogHelper.Info(typeof(LocationTypePropertyRepository), Msg);
        }

        protected override void PersistUpdatedItem(LocationTypeProperty item)
        {
            string Msg = string.Format("LocationTypeProperty '{0}' has been updated.", item.Name);

            item.UpdatingEntity();

            var converter = new DtoConverter();
            var dto = converter.ToLocationTypePropertyDto(item);

            Repositories.ThisDb.Update(dto);

            LogHelper.Info(typeof(LocationTypePropertyRepository), Msg);
        }

        protected override void PersistDeletedItem(LocationTypeProperty item, out StatusMessage StatusMsg)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = item.Name;

            ReturnMsg.Message = string.Format("LocationTypeProperty '{0}' has been deleted.", ReturnMsg.ObjectName);

            var converter = new DtoConverter();
            var dto = converter.ToLocationTypePropertyDto(item);

            Repositories.ThisDb.Delete(dto);
            ReturnMsg.Success = true;

            StatusMsg = ReturnMsg;
            //LogHelper.Info(typeof(LocationTypePropertyRepository), ReturnMsg.Message);
        }

        protected override Sql GetBaseQuery(bool isCount)
        {
            var MySql = new Sql();
            MySql.Select(isCount ? "COUNT(*)" : "*")
            .From<LocationTypePropertyDto>();
            return MySql;
        }

        /// <summary>
        /// Gets the base where clause
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
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
        //        if (Prop != null)
        //        {
        //            var PropDtId = Prop.DataTypeId;
        //            var MySql = new Sql();
        //            MySql.Select("*").From<cmsDataTypeDto>().Where<cmsDataTypeDto>(n => n.DataTypeId == PropDtId);

        //            var MatchingDt = Repositories.ThisDb.Fetch<cmsDataTypeDto>(MySql).FirstOrDefault();
        //            if (MatchingDt != null)
        //            {
        //                Prop.DatabaseType = MatchingDt.DatabaseType;
        //                Prop.PropertyEditorAlias = MatchingDt.PropertyEditorAlias;
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
