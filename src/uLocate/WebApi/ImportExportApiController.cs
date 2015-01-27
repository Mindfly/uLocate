namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using uLocate.Data;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// uLocate import/export api controller.
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class ImportExportApiController : UmbracoAuthorizedApiController
    {
        /// /umbraco/backoffice/uLocate/ImportExportApi/Test
        //[AcceptVerbs("GET")]
        //public bool Test()
        //{
        //    return true;
        //}

        /// <summary>
        /// Function to import locations from a csv file
        /// /umbraco/backoffice/uLocate/ImportExportApi/ImportLocationsCSV
        /// </summary>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public StatusMessage ImportLocationsCSV()
        {
            string file = "~/uLocateLocationImport.csv";
            return uLocate.IO.Import.LocationsCSV(file);
        }

        /// <summary>
        /// Function to import locations from a csv file
        /// /umbraco/backoffice/uLocate/ImportExportApi/ImportLocationsCSV?FileName=~/uLocateLocationImport.csv
        /// </summary>
        /// <param name="FileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public StatusMessage ImportLocationsCSV(string FileName)
        {
            string file;

            if (FileName == string.Empty)
            {
                file = "~/uLocateLocationImport.csv";
            }
            else
            {
                file = FileName;
            }
            return uLocate.IO.Import.LocationsCSV(file);
        }

        /// <summary>
        /// Function to import locations from a csv file
        /// /umbraco/backoffice/uLocate/ImportExportApi/ImportLocationsCSV?FileName=~/uLocateLocationImport.csv&LocationTypeKey=xxx
        /// </summary>
        /// <param name="FileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="StatusMessage"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public StatusMessage ImportLocationsCSV(string FileName, Guid LocationTypeKey)
        {
            string file;

            if (FileName == string.Empty)
            {
                file = "~/uLocateLocationImport.csv";
            }
            else
            {
                file = FileName;
            }
            return uLocate.IO.Import.LocationsCSV(file, LocationTypeKey);
        }

    }
}
