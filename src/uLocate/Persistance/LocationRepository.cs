namespace uLocate.Persistance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;

    using Mindfly;

    using uLocate.Data;
    using uLocate.Helpers;
    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Represents the <see cref="LocationRepository"/>.
    /// </summary>
    internal class LocationRepository : PagedRepositoryBase<Location> //, ILocationRespository
    {
        private List<Location> CurrentCollection = new List<Location>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRepository"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public LocationRepository(UmbracoDatabase database, IRuntimeCacheProvider cache)
            : base(database, cache)
        {
        }

        #region Public & Internal Methods

        public void Insert(Location Entity)
        {
            PersistNewItem(Entity);
        }

        public void Insert(Location Entity, out Guid NewItemKey)
        {
            //TODO: might not be needed
            PersistNewItem(Entity);
            NewItemKey = Entity.Key;
        }

        public StatusMessage Delete(Guid LocationKey)
        {
            StatusMessage ReturnMsg = new StatusMessage();

            Location ThisLoc = this.GetByKey(LocationKey);

            if (ThisLoc != null)
            {
                this.DeleteLocation(ThisLoc, out ReturnMsg);
            }
            else
            {
                ReturnMsg.Success = false;
                ReturnMsg.Code = "NotFound";
                ReturnMsg.ObjectName = LocationKey.ToString();
                ReturnMsg.Message = string.Format("Location with key '{0}' was not found and can not be deleted.", ReturnMsg.ObjectName);
            }

            return ReturnMsg;
        }

        public StatusMessage Delete(Location Entity)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            this.DeleteLocation(Entity, out ReturnMsg);

            return ReturnMsg;
        }

        private void DeleteLocation(Location Entity, out StatusMessage StatusMsg)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = Entity.Name;

            this.DeleteChildren(Entity);

            PersistDeletedItem(Entity, out ReturnMsg);
            StatusMsg = ReturnMsg;
        }
        
        public void Update(Location Entity)
        {
            PersistUpdatedItem(Entity);
        }

        public Location GetByKey(Guid Key)
        {
            CurrentCollection.Clear();
            CurrentCollection.Add((Location)Get(Key));
            FillChildren();

            return CurrentCollection[0];
        }

        public IEnumerable<Location> GetByName(string LocationName)
        {
            CurrentCollection.Clear();
            var sql = new Sql();
            sql.Select("*")
                .From<LocationDto>()
                .Where<LocationDto>(n => n.Name == LocationName);

            var dtoResult = Repositories.ThisDb.Fetch<LocationDto>(sql).ToList();

            var converter = new DtoConverter();
            CurrentCollection.AddRange(converter.ToLocationEntity(dtoResult));

            FillChildren();

            return CurrentCollection; 
        }

        internal IEnumerable<Location> GetByType(Guid LocationTypeKey)
        {
            CurrentCollection.Clear();
            var sql = new Sql();
            sql.Select("*")
                .From<LocationDto>()
                .Where<LocationDto>(n => n.LocationTypeKey == LocationTypeKey);

            var dtoResult = Repositories.ThisDb.Fetch<LocationDto>(sql).ToList();

            var converter = new DtoConverter();
            CurrentCollection.AddRange(converter.ToLocationEntity(dtoResult));

            FillChildren();

            return CurrentCollection; 
        }

        internal IEnumerable<Location> GetByGeoSearch(double SearchLat, double SearchLong, int MilesDistance)
        {
            CurrentCollection.Clear();
            var sql = new Sql();
            sql.Select("*")
                .From<LocationDto>()
                .Append(GeographyHelper.GetGeoSearchSql(SearchLat, SearchLong,  MilesDistance));

            var dtoResult = Repositories.ThisDb.Query<LocationDto>(sql).ToList();

            var converter = new DtoConverter();
            CurrentCollection.AddRange(converter.ToLocationEntity(dtoResult));

            FillChildren();

            return CurrentCollection;
        }

        internal IEnumerable<Location> GetByCustomQuery(Sql SqlQuery)
        {
            CurrentCollection.Clear();

            var dtoResult = Repositories.ThisDb.Query<LocationDto>(SqlQuery).ToList();

            var converter = new DtoConverter();
            CurrentCollection.AddRange(converter.ToLocationEntity(dtoResult));

            FillChildren();

            return CurrentCollection;
        }

        public IEnumerable<Location> GetByKey(Guid[] Keys)
        {
            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(Keys));
            FillChildren();

            return CurrentCollection;
        }

        public IEnumerable<Location> GetAll()
        {
            var EmptyParams = new Guid[] { };

            CurrentCollection.Clear();
            CurrentCollection.AddRange(GetAll(EmptyParams));
            FillChildren();

            return CurrentCollection;
        }

        public List<JsonLocation> GetPaged(long PageNumber, long ItemsPerPage, string WhereClause)
        {
            Sql sql = new Sql();
            sql.Append(
                "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY [Key]) AS Num, * FROM uLocate_Location) AS uLocateTable");
            sql.Append(
                "WHERE Num BETWEEN ((@PageNumber - 1) * @RowspPage + 1) AND (@PageNumber * @RowspPage)",
                new { PageNumber = PageNumber, RowspPage = ItemsPerPage });
            sql.Append("ORDER BY [Key];");

            CurrentCollection.Clear();
            var dtoResult = Repositories.ThisDb.Fetch<LocationDto>(sql).ToList();

            var converter = new DtoConverter();
            CurrentCollection.AddRange(converter.ToLocationEntity(dtoResult));

            FillChildren();
            var ReturnList = new List<JsonLocation>();

            foreach (var loc in CurrentCollection)
            {
                ReturnList.Add(new JsonLocation(loc));
            }

            return ReturnList;
        }

        public IEnumerable<JsonLocation> ConvertToJsonLocations(IEnumerable<Location> Locations)
        {
            var ReturnList = new List<JsonLocation>();

            foreach (var loc in Locations)
            {
                ReturnList.Add(new JsonLocation(loc));
            }

            return ReturnList;
        }

        public StatusMessage UpdateGeoForAllNeeded()
        {
            var ReturnMsg = new StatusMessage();

            var updateCounter = 0;
            var checkedCounter = 0;
            var dbCounter = 0;
            foreach (var location in this.GetAll())
            {
                if (location.Latitude == 0 | location.Longitude == 0)
                {
                    this.UpdateLatLong(location);
                    updateCounter++;
                }
                else if (location.DbGeogNeedsUpdated)
                {
                    this.UpdateDbGeography(location);
                    dbCounter++;
                }
                else if (!this.GeogIsValid(location))
                {
                    this.UpdateDbGeography(location);
                    dbCounter++;
                }

                checkedCounter++;
            }

            ReturnMsg.Success = true;
            ReturnMsg.Message =
                string.Format(
                    "{0} location(s) were checked for needing an update. {1} had their coordinates updated. Additionally, {2} had their database geography updated.",
                    checkedCounter,
                    updateCounter, 
                    dbCounter);

            return ReturnMsg;

        }

        public void UpdateLatLong(Location Loc)
        {
            var coord = DoGeocoding.GetCoordinateForAddress(Loc.Address);
            if (coord != null)
            {
                Loc.Coordinate = coord;
                Loc.Latitude = coord.Latitude;
                Loc.Longitude = coord.Longitude;
                Loc.GeocodeStatus = GeocodeStatus.Ok;
                this.Update(Loc);
                this.UpdateDbGeography(Loc);
            }
        }

        public void UpdateDbGeography(Location Loc)
        {
            var sql = new Sql();
            sql.Append("UPDATE [uLocate_Location]");
            sql.Append("SET [GeogCoordinate] = geography::Point([Latitude], [Longitude], 4326)");
            sql.Append(string.Format("WHERE  ([Key] = '{0}')", Loc.Key));

            Repositories.ThisDb.Execute(sql);

            Loc.DbGeogNeedsUpdated = false;
            this.Update(Loc);
        }

        public void UpdateWithNewProps(Guid LocationTypeKey)
        {
            var allLocations = this.GetByType(LocationTypeKey);

            foreach (var location in allLocations)
            {
                location.SyncPropertiesWithType();
                this.Update(location);
            }
        }

        private bool GeogIsValid(Location entity)
        {
            //TODO: make this more accurate - check for lat/long numbers within a certain precision from each other.

            bool Result = false;

            if (entity.Latitude == 0 || entity.Longitude == 0)
            {
                return Result;
            }
            else
            {
                 //trim or pad to a total of 7 chars + optional '-' sign
                var Lat = DataValuesHelper.TrimToSize(entity.Latitude, 7, true , true);
                var Long = DataValuesHelper.TrimToSize(entity.Longitude, 7, true, true);

                bool test = Long.StartsWith("-0.");
                string ValidMatchString = string.Concat(Lat, ",", Long);

                var sql = new Sql();
                sql.Append("SELECT TOP 1 CONCAT( [GeogCoordinate].Lat, ',', [GeogCoordinate].Long)");
                sql.Append("FROM [uLocate_Location]");
                sql.Append(string.Format("WHERE  ([Key] = '{0}')", entity.Key));

                var DbGeogString = Repositories.ThisDb.Query<string>(sql).FirstOrDefault();

                if (ValidMatchString == DbGeogString)
                {
                    Result = true;
                }

                return Result;
            }
        }

        

        internal void SetMaintenanceFlags()
        {
            var allLocations = this.GetAll();
            foreach (var location in allLocations)
            {
                SetMaintenanceFlags(location);
            }
        }

        internal void SetMaintenanceFlags(Location entity)
        {
            if (!this.GeogIsValid(entity))
            {
                entity.DbGeogNeedsUpdated = true;
            }
            
            this.Update(entity);
        }

        #endregion

        #region Protected Methods

        protected override IEnumerable<Location> PerformGetAll(params Guid[] Keys)
        {
            //TODO: Fix this - use cache + Dto
            List<Location> Result = new List<Location>();
            IEnumerable<LocationDto> dtoResults;

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
                sql.Select("*").From<LocationDto>();

                dtoResults = Repositories.ThisDb.Fetch<LocationDto>(sql).ToList();

                var converter = new DtoConverter();
                foreach (var result in dtoResults)
                {
                    Result.Add(converter.ToLocationEntity(result));
                }
            }

            return Result;
        }

        protected override Location PerformGet(Guid Key)
        {
            var sql = new Sql();
            sql
                .Select("*")
                .From<LocationDto>()
                .Where<LocationDto>(n => n.Key == Key);

            var dtoResult = Repositories.ThisDb.Fetch<LocationDto>(sql).FirstOrDefault();

            if (dtoResult == null)
                return null;

            var converter = new DtoConverter();
            var entity = converter.ToLocationEntity(dtoResult);

            return entity;
        }

        protected override void PersistNewItem(Location item)
        {
            string Msg = string.Format("Location '{0}' has been saved.", item.Name);

            item.AddingEntity();
            this.SyncProperties(item);

            var converter = new DtoConverter();
            var dto = converter.ToLocationDto(item);

            Repositories.ThisDb.Insert(dto);
            //item.Key = dto.Key;

            LogHelper.Info(typeof(LocationRepository), Msg);

            PersistChildren(item);
        }

        protected override void PersistUpdatedItem(Location item)
        {
            string Msg = string.Format("Location '{0}' has been updated.", item.Name);

            item.UpdatingEntity();

            var converter = new DtoConverter();
            var dto = converter.ToLocationDto(item);

            Repositories.ThisDb.Update(dto);

            foreach (var prop in item.PropertyData)
            {
                prop.UpdatingEntity();
                var pDto = converter.ToLocationPropertyDataDto(prop);
                Repositories.ThisDb.Update(pDto);
            }

            LogHelper.Info(typeof(LocationRepository), Msg);
        }

        protected override void PersistDeletedItem(Location item, out StatusMessage StatusMsg)
        {
            StatusMessage ReturnMsg = new StatusMessage();
            ReturnMsg.ObjectName = item.Name;

            DeleteChildren(item);
            ReturnMsg.Message = string.Format("LocationType '{0}' has been deleted.", ReturnMsg.ObjectName);

            var converter = new DtoConverter();
            var dto = converter.ToLocationDto(item);

            Repositories.ThisDb.Delete(dto);
            ReturnMsg.Success = true;

            StatusMsg = ReturnMsg;
            //LogHelper.Info(typeof(LocationTypeRepository), ReturnMsg.Message);
        }

        protected override Sql GetBaseQuery(bool isCount)
        {
            var MySql = new Sql();
            MySql
                .Select(isCount ? "COUNT(*)" : "*")
                .From<LocationDto>();
            // .InnerJoin<LocationType>()
            // .On<LocationDto, LocationType>(left => left.LocationTypeId, right => right.Id);

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
            //return "ulocateLocation.pk = @Key";
        }

        protected override IEnumerable<string> GetDeleteClauses()
        {
            var list = new List<string>
            {
                "DELETE FROM ulocateLocation WHERE pk = @Key"
            };

            return list;
        }

        #endregion

        #region Private Methods
        private void FillChildren()
        {
            this.FillProperties();
            
        }

        private void PersistChildren(Location item)
        {
            this.PersistProperties(item);
        }

        private void DeleteChildren(Location item)
        {
            this.DeleteProperties(item);
        }

        private void FillProperties()
        {
            //var LtpRepo = Repositories.LocationTypePropertyRepo;
            var LocDataRepo  = Repositories.LocationPropertyDataRepo;

            foreach (var Location in CurrentCollection)
            {
                Location.PropertyData = LocDataRepo.GetByLocation(Location.Key).ToList();
            }
        }

        private void PersistProperties(Location item)
        {
            this.SyncProperties(item);
            var Repo = Repositories.LocationPropertyDataRepo;
            foreach (var NewProp in item.PropertyData)
            {
                if (NewProp.LocationKey == Guid.Empty)
                {
                    NewProp.LocationKey = item.Key;
                }

                Repo.Insert(NewProp);
            }
        }

        private void DeleteProperties(Location item)
        {
            var Repo = new LocationPropertyDataRepository(Repositories.ThisDb, Helper.ThisCache);
            var MatchingProps = Repo.GetByLocation(item.Key);

            foreach (var Prop in MatchingProps)
            {
                Repo.Delete(Prop);
            }
        }

        private void SyncProperties(Location item)
        {
            //Get location type properties
            var AllTypeProperties = Repositories.LocationTypePropertyRepo.GetByLocationType(item.LocationTypeKey);
            
            //Get location property data
            List<LocationPropertyData> AllData = item.PropertyData.ToList();
            List<Guid> PropsToAdd = new List<Guid>();
            //compare type properties with location data
            foreach (var typeProperty in AllTypeProperties)
            {
                var MatchingProp = AllData.Where(p => p.LocationTypePropertyKey == typeProperty.Key);
                if (!MatchingProp.Any())
                {
                    PropsToAdd.Add(typeProperty.Key);
                }
            }

            foreach (var propKey in PropsToAdd)
            {
                var NewProp = new LocationPropertyData(item.Key, propKey);
                AllData.Add(NewProp);
            }


            //Add missing properties to Location with blank values

        }


        #endregion

        public override Page<Location> Page(long page, long itemsPerPage, Sql sql)
        {
            throw new NotImplementedException();
        }
    }
}