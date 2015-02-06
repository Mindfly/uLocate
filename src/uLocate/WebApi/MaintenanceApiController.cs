namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using uLocate.Configuration;
    using uLocate.Data;
    using uLocate.Helpers;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The location type api controller for use by the umbraco back-office
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class MaintenanceApiController : UmbracoAuthorizedApiController
    {
        /// /umbraco/backoffice/uLocate/MaintenanceApi/Test
        [System.Web.Http.AcceptVerbs("GET")]
        public bool Test()
        {
            return true;
        }

        /// <summary>
        /// Gets a list of all countries and their codes.
        /// /umbraco/backoffice/uLocate/MaintenanceApi/GetCountryCodes
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable"/> of <see cref="Country"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public IEnumerable<Country> GetCountryCodes()
        {
            return CountryHelper.GetAllCountries();
        }


    }
}

