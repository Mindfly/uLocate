namespace Controllers.Api{
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;

/// <summary>
/// Summary description for fileUploadTestApiController
/// </summary>
public class FileUploadTestApiController : UmbracoApiController
{
    
    public async Task<HttpResponseMessage> PostStuff()
	{
        if (!Request.Content.IsMimeMultipartContent())
        {
            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        }

        var root = HttpContext.Current.Server.MapPath("~/App_Data/Temp/FileUploads");
        Directory.CreateDirectory(root);
        var provider = new MultipartFormDataStreamProvider(root);
        var result = await Request.Content.ReadAsMultipartAsync(provider);
        if (result.FormData["locationType"] == null)
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        var model = result.FormData["locationType"];
        // TODO: Do something with the json model which is currently a string


        var fileName = string.Empty;
        // get the files
        foreach (var file in result.FileData)
        {
            fileName = file.LocalFileName;
            // TODO: Do something with each uploaded file
        }

        return Request.CreateResponse(HttpStatusCode.OK, "success!");
	}
}
}