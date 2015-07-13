namespace uLocate.UI.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    using AutoMapper;

    using Umbraco.Core.Models;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.Models.ContentEditing;
    using Umbraco.Web.WebApi;

    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class DataTypeApiController : UmbracoAuthorizedApiController
    {
        //private LocationService locationService = new LocationService();
        //private LocationTypeService locationTypeService = new LocationTypeService();

        ///// /umbraco/backoffice/uLocate/DataTypeApi/Test
        //[System.Web.Http.AcceptVerbs("GET")]
        //public bool Test()
        //{
        //    return true;
        //}

        /// <summary>
        /// Gets all the DataTypes in this umbraco install.
        /// /umbraco/backoffice/uLocate/DataTypeApi/GetAllDataTypesWithGuids
        /// </summary>
        /// <returns>IEnumerable of GUID & DataType Name</returns>
        public object GetAllDataTypesWithGuids()
        {
            var dataTypes = this.Services.DataTypeService.GetAllDataTypeDefinitions();
            return dataTypes.Select(t => new { guid = t.Key, name = t.Name });
        }

        /// <summary>
        /// Get all uLocate Allowed DataTypes in this Umbraco install (based on the uLocate configuration)
        /// /umbraco/backoffice/uLocate/DataTypeApi/GetAllAvailable
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary"/> of DataTyp.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public Dictionary<int, string> GetAllAvailable()
        {
            return Data.Helper.GetAllAllowedDataTypes();
        }

        /// <summary>
        /// Get information about a DatatYpe by its name.
        /// /umbraco/backoffice/uLocate/DataTypeApi/GetByName?name=xxx
        /// </summary>
        /// <param name="name">
        /// The name to lookup
        /// </param>
        /// <returns>
        /// An object of type <see cref="DataTypeDisplay"/> or an error. 
        /// </returns>      
        [System.Web.Http.AcceptVerbs("GET")] 
        public object GetByName(string name)
        {
            var all = this.Services.DataTypeService.GetAllDataTypeDefinitions().ToList();
            var dataType = all.FirstOrDefault(x => x.Name == name);
            return this.FormatDataType(dataType);
        }

        //public object GetByKey(string key)
        //{
            
        //}

        /// <summary>
        /// The format data type.
        /// </summary>
        /// <param name="dtd">
        /// The dtd.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="HttpResponseException">
        /// </exception>
        protected object FormatDataType(IDataTypeDefinition dtd)
        {
            if (dtd == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var dataTypeDisplay = Mapper.Map<IDataTypeDefinition, DataTypeDisplay>(dtd);
            var propEditor = PropertyEditorResolver.Current.GetByAlias(dtd.PropertyEditorAlias);

            var configDictionairy = new Dictionary<string, object>();

            foreach (var pv in dataTypeDisplay.PreValues)
            {
                configDictionairy.Add(pv.Key, pv.Value);
            }

            return new
            {
                guid = dtd.Key,
                propertyEditorAlias = dtd.PropertyEditorAlias,
                config = configDictionairy,
                view = propEditor.ValueEditor.View
            };
        }


    }
}
