namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    using AutoMapper;

    using uLocate.Data;
    using uLocate.Persistance;
    using uLocate.Providers;

    using Umbraco.Core.Models;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.Models.ContentEditing;
    using Umbraco.Web.WebApi;

    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class DataTypeApiController : UmbracoAuthorizedApiController
    {
        ///// /umbraco/backoffice/uLocate/DataTypeApi/Test
        //[System.Web.Http.AcceptVerbs("GET")]
        //public bool Test()
        //{
        //    return true;
        //}

        public object GetAllDataTypesWithGuids()
        {
            var dataTypes = Services.DataTypeService.GetAllDataTypeDefinitions();
            return dataTypes.Select(t => new { guid = t.Key, name = t.Name });
        }

        /// /umbraco/backoffice/uLocate/DataTypeApi/GetAllAvailable
        [System.Web.Http.AcceptVerbs("GET")]
        public Dictionary<int, string> GetAllAvailable()
        {
            return Data.Helper.GetAllAllowedDataTypes();
        }

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTypeDisplay"/>.
        /// </returns>      
        [System.Web.Http.AcceptVerbs("GET")] 
        public object GetByName(string name)
        {
            var all = DataTypeCacheProvider.Current.GetOrExecute(() => this.Services.DataTypeService.GetAllDataTypeDefinitions().ToList());
            var dataType = all.FirstOrDefault(x => x.Name == name);
            return this.FormatDataType(dataType);
        }

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
