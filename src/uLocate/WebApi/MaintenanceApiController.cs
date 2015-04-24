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
    using Umbraco.Core.Persistence;
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

        #region Operations

        /// <summary>
        /// Updates Lat/Long coordinates for all Locations which require it.
        /// /umbraco/backoffice/uLocate/MaintenanceApi/UpdateCoordinatesAsNeeded
        /// </summary>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public StatusMessage UpdateCoordinatesAsNeeded()
        {
            var Result = Repositories.LocationRepo.UpdateGeoForAllNeeded();

            return Result;
        }

        /// <summary>
        /// Delete all locations with a matching name
        /// /umbraco/backoffice/uLocate/MaintenanceApi/DeleteLocationsByName?LocName=xxx
        /// </summary>
        /// <param name="LocName">
        /// The loc name.
        /// </param>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public StatusMessage DeleteLocationsByName(string LocName)
        {
            var Msg = new StatusMessage();
            Msg.ObjectName = LocName;
            

            var matchingLocations = Repositories.LocationRepo.GetByName(LocName);
            if (matchingLocations.Any())
            {
                foreach (var loc in matchingLocations)
                {
                    Repositories.LocationRepo.Delete(loc);
                }

                Msg.Message = string.Format("{0} location(s) named '{1}' were found and deleted.", matchingLocations.Count(), LocName);
            }
            else
            {
                Msg.Message = string.Format("No locations named '{0}' were found.", LocName);
            }
            Msg.Success = true;
            return Msg;
        }

        #endregion

        #region Querying
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

        /// <summary>
        /// Gets information about which Locations need their geography updated
        /// /umbraco/backoffice/uLocate/MaintenanceApi/GeographyNeedsUpdated
        /// </summary>
        /// <returns>
        /// The <see cref="MaintenanceCollection"/>.
        /// </returns>
        [System.Web.Http.AcceptVerbs("GET")]
        public MaintenanceCollection GeographyNeedsUpdated()
        {
            //Repositories.LocationRepo.SetMaintenanceFlags();

            var maintColl = new MaintenanceCollection();
            maintColl.Title = "Database 'Geography' Data Needs Updated";

            maintColl.Locations = Repositories.LocationRepo.GetAllMissingDbGeo();
            maintColl.ConvertToJsonLocationsOnly();

            return maintColl;
        }
        
        #endregion

    }
}

