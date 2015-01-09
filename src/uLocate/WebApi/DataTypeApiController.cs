namespace uLocate.WebApi
{
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Data;
    using uLocate.Persistance;

    using Umbraco.Core.Persistence;
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

        /// /umbraco/backoffice/uLocate/DataTypeApi/GetAllAvailable
        [System.Web.Http.AcceptVerbs("GET")]
        public Dictionary<int, string> GetAllAvailable()
        {
            return Data.Helper.GetAllAllowedDataTypes();
        }
    }
}
