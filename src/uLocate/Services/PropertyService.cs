namespace uLocate.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;

    /// <summary>
    /// Functions related to looking up and editing data which can be called from Api Controllers etc.
    /// </summary>
    public class PropertyService
    {
        public LocationTypeProperty GetProperty(string PropertyAlias)
        {
            var result = Repositories.LocationTypePropertyRepo.GetByAlias(PropertyAlias);

            return result;
        }

        public Dictionary<int, string> GetPropertyPreValuesDictionary(string PropertyAlias)
        {
            var preValues = GetPropertyPreValuesCollection(PropertyAlias);

            var result = new Dictionary<int, string>();

            var items = preValues.PreValuesAsArray;
            foreach (var item in items)
            {
                result.Add(item.Id, item.Value);
            }

            return result;
        }

        public PreValueCollection GetPropertyPreValuesCollection(string PropertyAlias)
        {
            var property = Repositories.LocationTypePropertyRepo.GetByAlias(PropertyAlias);
            var dtId = property.DataType.DataTypeId;

            var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
            var result = dataTypeService.GetPreValuesCollectionByDataTypeId(dtId);

            return result;
        }

        //#region Location-Related

        //public EditableLocation GetLocation(Guid LocationKey)
        //{
        //    var Result = Repositories.LocationRepo.GetByKey(LocationKey);

        //    return Result;
        //}

        //public StatusMessage DeleteLocation(Guid LocationKey)
        //{
        //    var Result = Repositories.LocationRepo.Delete(LocationKey);

        //    return Result;
        //}

        //public Guid CreateLocation(string LocationName, bool UpdateIfFound = false)
        //{
        //    var Result = CreateLocation(LocationName, uLocate.Constants.DefaultLocationTypeKey, UpdateIfFound);

        //    return Result;
        //}

        //public Guid CreateLocation(string LocationName, Guid LocationTypeGuid, bool UpdateIfFound = false)
        //{
        //    bool DoUpdate = false;
        //    Guid LocType;

        //    if (LocationTypeGuid != Guid.Empty)
        //    {
        //        LocType = LocationTypeGuid;
        //    }
        //    else
        //    {
        //        LocType = uLocate.Constants.DefaultLocationTypeKey;
        //    }

        //    if (UpdateIfFound)
        //    {
        //        //Lookup first
        //        var matchingLocations = Repositories.LocationRepo.GetByName(LocationName);
        //        if (matchingLocations.Any())
        //        {
        //            EditableLocation lookupLoc = matchingLocations.FirstOrDefault();
        //            lookupLoc.Name = LocationName;
        //            lookupLoc.LocationTypeKey = LocType;
        //            Repositories.LocationRepo.Update(lookupLoc);
        //            return lookupLoc.Key;
        //        }
        //    }

        //    EditableLocation newLoc = new EditableLocation(LocationName, LocType);
        //    Repositories.LocationRepo.Insert(newLoc);
        //    return newLoc.Key;
        //}

        //public EditableLocation UpdateLocation(EditableLocation UpdatedLocation, bool UpdateIfFound = false)
        //{
        //    Repositories.LocationRepo.Update(UpdatedLocation);

        //    var Result = Repositories.LocationRepo.GetByKey(UpdatedLocation.Key);

        //    return Result;
        //}

        //#endregion

        //#region Location Collections

        //public IEnumerable<EditableLocation> GetLocations()
        //{
        //    var result = Repositories.LocationRepo.GetAll();

        //    return result;
        //}

        //public IEnumerable<EditableLocation> GetLocations(Guid LocationTypeKey)
        //{
        //    var result = Repositories.LocationRepo.GetByType(LocationTypeKey);

        //    return result;
        //}

        //public IEnumerable<EditableLocation> GetLocationsByPropertyValue(string PropertyAlias, string Value)
        //{
        //    var AllLocations = Repositories.LocationRepo.GetAll();
            
        //    var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value);

        //    return result;
        //}

        //public IEnumerable<EditableLocation> GetLocationsByPropertyValue(string PropertyAlias, int Value)
        //{
        //    var AllLocations = Repositories.LocationRepo.GetAll();

        //    var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

        //    return result;
        //}

        //public IEnumerable<EditableLocation> GetLocationsByPropertyValue(string PropertyAlias, DateTime Value)
        //{
        //    var AllLocations = Repositories.LocationRepo.GetAll();

        //    var result = AllLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

        //    return result;
        //}

        //#endregion
    }

    
}
