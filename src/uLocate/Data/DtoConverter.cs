namespace uLocate.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Helpers;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core.Persistence;

    /// <summary>
    /// Converts between Dto (for database access/storage) and Model Entities
    /// </summary>
    internal class DtoConverter
    {
        #region LocationType
        public LocationType ToLocationTypeEntity(LocationTypeDto dto)
        {
            var Entity = new LocationType()
            {
                Key = dto.Key,
                Name = dto.Name,
                Description = dto.Description,
                Icon = dto.Icon,
                UpdateDate = dto.UpdateDate,
                CreateDate = dto.CreateDate
            };

            return Entity;
        }

        public LocationTypeDto ToLocationTypeDto(LocationType entity)
        {
            var dto = new LocationTypeDto()
            {
                Key = entity.Key,
                Name = entity.Name,
                Description = entity.Description,
                Icon = entity.Icon,
                UpdateDate = entity.UpdateDate,
                CreateDate = entity.CreateDate
            };

            return dto;
        }

        public IEnumerable<LocationType> ToLocationTypeEntity(IEnumerable<LocationTypeDto> DtoCollection)
        {
            List<LocationType> Result = new List<LocationType>();

            foreach (var item in DtoCollection)
            {
                Result.Add(this.ToLocationTypeEntity(item));
            }

            return Result;
        }

        public IEnumerable<LocationTypeDto> ToLocationTypeDto(IEnumerable<LocationType> EntityCollection)
        {
            List<LocationTypeDto> Result = new List<LocationTypeDto>();

            foreach (var item in EntityCollection)
            {
                Result.Add(this.ToLocationTypeDto(item));
            }

            return Result;
        }

        #endregion

        #region LocationTypeProperty

        public LocationTypeProperty ToLocationTypePropertyEntity(LocationTypePropertyDto dto)
        {
            var Entity = new LocationTypeProperty()
            {
                Key = dto.Key,
                Alias = dto.Alias,
                Name = dto.Name,
                DataTypeId = dto.DataTypeId,
                DataType = this.GetDataType(dto.DataTypeId),
                LocationTypeKey = dto.LocationTypeKey,
                SortOrder = dto.SortOrder,
                UpdateDate = dto.UpdateDate,
                CreateDate = dto.CreateDate,
                IsDefaultProp = dto.IsDefaultProp
            };

            return Entity;
        }

        private CmsDataType GetDataType(int DataTypeId)
        {
            var returnDt = new CmsDataType();
            //var dataTypeId = this.DataTypeId;
            var sql = new Sql();
            sql
                .Select("*")
                .From<cmsDataTypeDto>()
                .Where<cmsDataTypeDto>(n => n.DataTypeId == DataTypeId);

            var MatchingDt = Repositories.ThisDb.Fetch<cmsDataTypeDto>(sql).FirstOrDefault();
            if (MatchingDt != null)
            {
                returnDt.Key = MatchingDt.Key;
                returnDt.DataTypeId = MatchingDt.DataTypeId;
                returnDt.DatabaseTypeString = MatchingDt.DatabaseType;
                returnDt.PropertyEditorAlias = MatchingDt.PropertyEditorAlias;
            }

            return returnDt;
        }

        public LocationTypePropertyDto ToLocationTypePropertyDto(LocationTypeProperty entity)
        {
            var dto = new LocationTypePropertyDto()
            {
                Key = entity.Key,
                Alias = entity.Alias,
                Name = entity.Name,
                DataTypeId = entity.DataTypeId,
                LocationTypeKey = entity.LocationTypeKey,
                SortOrder = entity.SortOrder,
                UpdateDate = entity.UpdateDate,
                CreateDate = entity.CreateDate,
                IsDefaultProp = entity.IsDefaultProp
            };

            return dto;
        }

        public IEnumerable<LocationTypeProperty> ToLocationTypePropertyEntity(IEnumerable<LocationTypePropertyDto> DtoCollection)
        {
            List<LocationTypeProperty> Result = new List<LocationTypeProperty>();

            foreach (var item in DtoCollection)
            {
                Result.Add(this.ToLocationTypePropertyEntity(item));
            }

            return Result;
        }

        public IEnumerable<LocationTypePropertyDto> ToLocationTypePropertyDto(IEnumerable<LocationTypeProperty> EntityCollection)
        {
            List<LocationTypePropertyDto> Result = new List<LocationTypePropertyDto>();

            foreach (var item in EntityCollection)
            {
                Result.Add(this.ToLocationTypePropertyDto(item));
            }

            return Result;
        }

        #endregion

        #region EditableLocation

        public EditableLocation ToLocationEntity(LocationDto dto)
        {
            var Entity = new EditableLocation()
            {
                Key = dto.Key,
                Name = dto.Name,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Coordinate = new Coordinate(dto.Latitude, dto.Longitude),
                GeocodeStatus = DoGeocoding.GetGeocodeStatus(dto.GeocodeStatus),
                DbGeogNeedsUpdated = dto.DbGeogNeedsUpdated,
                LocationTypeKey = dto.LocationTypeKey,
                UpdateDate = dto.UpdateDate,
                CreateDate = dto.CreateDate
                //Viewport = new Viewport(dto.Viewport),
            };

            return Entity;
        }

        public LocationDto ToLocationDto(EditableLocation entity)
        {
            var dto = new LocationDto()
            {
                Key = entity.Key,
                Name = entity.Name,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                GeocodeStatus = entity.GeocodeStatus.ToString(),
                DbGeogNeedsUpdated = entity.DbGeogNeedsUpdated,
                LocationTypeKey = entity.LocationTypeKey,
                UpdateDate = entity.UpdateDate,
                CreateDate = entity.CreateDate
                //Viewport = entity.Viewport.ToString(),
            };

            return dto;
        }

       

        public IEnumerable<EditableLocation> ToLocationEntity(IEnumerable<LocationDto> DtoCollection)
        {
            List<EditableLocation> Result = new List<EditableLocation>();

            foreach (var item in DtoCollection)
            {
                Result.Add(this.ToLocationEntity(item));
            }

            return Result;
        }

        public IEnumerable<LocationDto> ToLocationDto(IEnumerable<EditableLocation> EntityCollection)
        {
            List<LocationDto> Result = new List<LocationDto>();

            foreach (var item in EntityCollection)
            {
                Result.Add(this.ToLocationDto(item));
            }

            return Result;
        }

        #endregion

        #region LocationPropertyData

        public LocationPropertyData ToLocationPropertyDataEntity(LocationPropertyDataDto dto)
        {
            var Entity = new LocationPropertyData()
            {
                Key = dto.Key,
                LocationKey = dto.LocationKey,
                LocationTypePropertyKey = dto.LocationTypePropertyKey,
                dataInt = dto.dataInt,
                dataDate = dto.dataDate,
                dataNvarchar = dto.dataNvarchar,
                dataNtext = dto.dataNtext,
                UpdateDate = dto.UpdateDate,
                CreateDate = dto.CreateDate
            };

            return Entity;
        }

        public LocationPropertyDataDto ToLocationPropertyDataDto(LocationPropertyData entity)
        {
            var dto = new LocationPropertyDataDto()
            {
                Key = entity.Key,
                LocationKey = entity.LocationKey,
                LocationTypePropertyKey = entity.LocationTypePropertyKey,
                dataInt = entity.dataInt,
                dataDate = entity.dataDate,
                dataNvarchar = entity.dataNvarchar,
                dataNtext = entity.dataNtext,
                UpdateDate = entity.UpdateDate,
                CreateDate = entity.CreateDate
            };

            return dto;
        }

        public IEnumerable<LocationPropertyData> ToLocationPropertyDataEntity(IEnumerable<LocationPropertyDataDto> DtoCollection)
        {
            List<LocationPropertyData> Result = new List<LocationPropertyData>();

            foreach (var item in DtoCollection)
            {
                Result.Add(this.ToLocationPropertyDataEntity(item));
            }

            return Result;
        }

        public IEnumerable<LocationPropertyDataDto> ToLocationTypePropertyDto(IEnumerable<LocationPropertyData> EntityCollection)
        {
            List<LocationPropertyDataDto> Result = new List<LocationPropertyDataDto>();

            foreach (var item in EntityCollection)
            {
                Result.Add(this.ToLocationPropertyDataDto(item));
            }

            return Result;
        }

        #endregion
    }
}
