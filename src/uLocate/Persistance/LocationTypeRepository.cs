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

    using Constants = uLocate.Constants;

    /// <summary>
    /// Represents the <see cref="LocationTypeRepository"/>.
    /// </summary>
    internal class LocationTypeRepository : RepositoryBase<LocationType> 
    {
        /// <summary>
        /// The current collection of matched entities
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

        public void Insert(LocationType Entity, out Guid NewItemKey)
        {
            //TODO: might not be needed
            PersistNewItem(Entity);
            NewItemKey = Entity.Key;
        }

        public StatusMessage Delete(Guid LocationTypeKey, bool ConvertLocationsToDefault = true)
        {
            StatusMessage ReturnMsg = new StatusMessage();

            LocationType ThisLocType = this.GetByKey(LocationTypeKey);

            if (ThisLocType != null)
            {
                this.DeleteLocType(ThisLocType,  out ReturnMsg, ConvertLocationsToDefault);
            }
            else
            {
                ReturnMsg.Success = false;
                ReturnMsg.Code = "NotFound";
                ReturnMsg.ObjectName = LocationTypeKey.ToString();
                ReturnMsg.Message = string.Format("Location Type with key '{0}' was not found and can not be deleted.", ReturnMsg.ObjectName);
            }

            return ReturnMsg;
        }

        public StatusMessage Delete(LocationType Entity,bool ConvertLocationsToDefault = true)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            this.DeleteLocType(Entity, out ReturnMsg, ConvertLocationsToDefault);

            return ReturnMsg;
        }

        private void DeleteLocType(LocationType Entity, out StatusMessage StatusMsg, bool ConvertLocationsToDefault = true)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = Entity.Name;

            if (ConvertLocationsToDefault)
            {
                //Check for associated locations and update them to use LocationTypeId = DefaultId
                var MatchingLocs = Repositories.LocationRepo.GetByType(Entity.Key);

                foreach (var location in MatchingLocs)
                {
                    location.LocationTypeKey = Constants.DefaultLocationTypeKey;
                    Repositories.LocationRepo.Update(location);
                }
            }
            //else matching Locations will be deleted with children
            
            this.DeleteChildren(Entity);
        
            PersistDeletedItem(Entity, out ReturnMsg);
            StatusMsg = ReturnMsg;
        }
        
        public void Update(LocationType Entity)
        {
            PersistUpdatedItem(Entity);
        }

        public IEnumerable<LocationType> GetByName(string LocationTypeName)
        {
            CurrentCollection.Clear();
            var sql = new Sql();
            sql.Select("*")
                .From<LocationTypeDto>()
                .Where<LocationTypeDto>(n => n.Name == LocationTypeName);


            var dtoResult = Repositories.ThisDb.Fetch<LocationTypeDto>(sql).ToList();

            var converter = new DtoConverter();
            CurrentCollection.AddRange(converter.ToLocationTypeEntity(dtoResult));

            FillChildren();

            return CurrentCollection; 
        }

        public LocationType GetByKey(Guid Key)
        {
            CurrentCollection.Clear();
            CurrentCollection.Add((LocationType)Get(Key));
            FillChildren();
            if (CurrentCollection.Count != 0)
            {
                return CurrentCollection[0];
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<LocationType> GetByKey(Guid[] Keys)
        {
            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(Keys));
            FillChildren();

            return CurrentCollection; 
        }

        public IEnumerable<LocationType> GetAll()
        {
            var EmptyParams = new Guid[] { };

            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(EmptyParams));
            FillChildren();

            return CurrentCollection; 
        }

        #endregion

        #region Protected Methods

        protected override IEnumerable<LocationType> PerformGetAll(params Guid[] Keys)
        {
            List<LocationType> Result = new List<LocationType>();
            IEnumerable<LocationTypeDto> dtoResults;

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
                sql.Select("*").From<LocationTypeDto>();

                dtoResults = Repositories.ThisDb.Fetch<LocationTypeDto>(sql).ToList();

                var converter = new DtoConverter();
                foreach (var result in dtoResults)
                {
                    Result.Add(converter.ToLocationTypeEntity(result));
                }
            }

            return Result;
        }

        protected override LocationType PerformGet(Guid Key)
        {
            var sql = new Sql();
            sql
                .Select("*")
                .From<LocationTypeDto>()
                .Where<LocationTypeDto>(n => n.Key == Key);

            var dtoResult = Repositories.ThisDb.Fetch<LocationTypeDto>(sql).FirstOrDefault();

            if (dtoResult == null)
                return null;

            var converter = new DtoConverter();
            var entity = converter.ToLocationTypeEntity(dtoResult);

            return entity;
        }

        protected override void PersistNewItem(LocationType item)
        {
            string Msg = string.Format("LocationType '{0}' has been saved.", item.Name);

            item.AddingEntity();

            var converter = new DtoConverter();
            var dto = converter.ToLocationTypeDto(item);

            Repositories.ThisDb.Insert(dto);
            item.Key = dto.Key;

            LogHelper.Info(typeof(LocationTypeRepository), Msg);
            
            PersistChildren(item);


        }

        protected override void PersistUpdatedItem(LocationType item)
        {
            string Msg = string.Format("LocationType '{0}' has been updated.", item.Name);

            item.UpdatingEntity();

            var converter = new DtoConverter();
            var dto = converter.ToLocationTypeDto(item);

            Repositories.ThisDb.Update(dto);

            foreach (var prop in item.Properties)
            {
                prop.UpdatingEntity();
                var pDto = converter.ToLocationTypePropertyDto(prop);
                Repositories.ThisDb.Update(pDto);
            }
            
            LogHelper.Info(typeof(LocationTypeRepository), Msg);
        }

        protected override void PersistDeletedItem(LocationType item, out StatusMessage StatusMsg)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = item.Name;

            if (item.Key != Constants.DefaultLocationTypeKey)
            {
                DeleteChildren(item);
                ReturnMsg.Message = string.Format("LocationType '{0}' has been deleted.", ReturnMsg.ObjectName);
                var converter = new DtoConverter();
                var dto = converter.ToLocationTypeDto(item);

                Repositories.ThisDb.Delete(dto);
                ReturnMsg.Success = true;
            }
            else
            {
                ReturnMsg.Message = string.Format("LocationType '{0}' is the default location type and cannot be deleted.", item.Name);
                ReturnMsg.Code = "Prohibited";
                ReturnMsg.Success = false;
            }

            StatusMsg = ReturnMsg;
            LogHelper.Info(typeof(LocationTypeRepository), ReturnMsg.Message);
        }

        protected override Sql GetBaseQuery(bool isCount)
        {
            var MySql = new Sql();
            MySql.Select(isCount ? "COUNT(*)" : "*")
			.From<LocationTypeDto>();
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

        private void DeleteChildren(LocationType item)
        {
            this.DeleteProperties(item);
            this.DeleteAssociatedLocations(item);
        }

        private void DeleteAssociatedLocations(LocationType item)
        {
            //find locations by type
            StatusMessage StatusMsg = new StatusMessage();
            StatusMsg.ObjectName = item.Name;

            var MatchingLocations = Repositories.LocationRepo.GetByType(item.Key);
            if (MatchingLocations.Any())
            {
                foreach (var loc in MatchingLocations)
                {
                    StatusMsg = Repositories.LocationRepo.Delete(loc);
                }
                
            }
        }

        private void FillProperties()
        {
            var Repo = new LocationTypePropertyRepository(Repositories.ThisDb, Helper.ThisCache);
            foreach (var LocType in CurrentCollection)
            {
                LocType.Properties = Repo.GetByLocationType(LocType.Key).ToList();
            }
        }

        private void PersistProperties(LocationType item)
        {
            var Repo = new LocationTypePropertyRepository(Repositories.ThisDb, Helper.ThisCache);
            foreach (var NewProp in item.Properties)
            {
                if (NewProp.LocationTypeKey == Guid.Empty)
                {
                    NewProp.LocationTypeKey = item.Key;
                }

                Repo.Insert(NewProp);
            }
        }

        private void DeleteProperties(LocationType item)
        {
            var Repo = new LocationTypePropertyRepository(Repositories.ThisDb, Helper.ThisCache);
            var MatchingProps = Repo.GetByLocationType(item.Key);

            foreach (var Prop in MatchingProps)
            {
                Repo.Delete(Prop);
            }
        }

        #endregion
    }
}