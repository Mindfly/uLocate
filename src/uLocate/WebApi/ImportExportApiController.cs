namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
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

        #region Import Functions

        /// <summary>
        /// Function to import locations from a csv file located at "~/uLocateLocationImport.csv"
        /// /umbraco/backoffice/uLocate/ImportExportApi/ImportLocationsCSV
        /// </summary>
        /// <returns>
        /// An object of type <see cref="StatusMessage"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public StatusMessage ImportLocationsCSV()
        {
            string file = "~/uLocateLocationImport.csv";
            
            return uLocate.IO.Import.LocationsCSV(file);
        }

        /// <summary>
        /// Function to import locations from a csv file located at "~/uLocateLocationImport.csv"
        /// /umbraco/backoffice/uLocate/ImportExportApi/ImportLocationsCSV?LocationTypeKey=xxx
        /// </summary>
        /// <param name="LocationTypeKey">
        /// The GUID key of the Location Type to import
        /// </param>
        /// <returns>
        /// An object of type <see cref="StatusMessage"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public StatusMessage ImportLocationsCSV(Guid LocationTypeKey)
        {
            string file = "~/uLocateLocationImport.csv";

            return uLocate.IO.Import.LocationsCSV(file, LocationTypeKey);
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

            //if (file.EndsWith(".csv"))
            //{
            //    
            //}
            //else
            //{
            //    var msg =
            //        string.Format(
            //            "The file uploaded ({0}) is not a CSV (comma-separated values) file. Please try again.",
            //            file);
            //    return new StatusMessage(false, msg) { Code = "WrongFileType", ObjectName = file, };
            //}
        }

        /// <summary>
        /// The Upload File To Server function.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="HttpResponseException">
        /// Something something.
        /// </exception>
        public async Task<HttpResponseMessage> UploadFileToServer()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data/Temp/FileUploads");
            Directory.CreateDirectory(root);
            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            var fileName = string.Empty;

            // get the files
            foreach (var file in result.FileData)
            {
                fileName += "," + file.LocalFileName;
            }

            return Request.CreateResponse(HttpStatusCode.OK, fileName);
        }
        
        #endregion

        #region Export Functions

        /// <summary> 
        /// /umbraco/backoffice/uLocate/ImportExportApi/GetListofColumnHeaders?LocationTypeKey=xxx
        /// </summary>
        [AcceptVerbs("GET")]
        public string GetListofColumnHeaders(Guid LocationTypeKey)
        {
            return uLocate.IO.Export.GetListofColumnHeaders(LocationTypeKey);
        }

        /// <summary> 
        /// /umbraco/backoffice/uLocate/ImportExportApi/ExportAllLocations?LocationTypeKey=xxx
        /// </summary>
        [AcceptVerbs("GET")]
        public StatusMessage ExportAllLocations(Guid LocationTypeKey)
        {
            return uLocate.IO.Export.ExportAllLocations(LocationTypeKey);
        }

        #endregion

    }
}
