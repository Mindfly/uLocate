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
    }
}
