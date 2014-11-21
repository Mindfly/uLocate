using System.IO;
using System.Net;
using System.Xml.Linq;
using umbraco.cms.businesslogic;

namespace uLocate.Plugins.Geocode.GoogleMaps
{
    using System;
    using System.Linq;
    using System.Web;
    using Caching;
    using Models;
    using Providers;
    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;

    /// <summary>
    /// The google maps geocode provider.
    /// </summary>
    [GeocodeProvider("googleMapsAPIProvider", "Google Maps Geocode Provider")]
    public class GoogleMapsGeocodeProvider : GeocodeProviderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapsGeocodeProvider"/> class.
        /// </summary>
        public GoogleMapsGeocodeProvider()
            : this(ApplicationContext.Current != null ? ApplicationContext.Current.ApplicationCache.RuntimeCache : new NullCacheProvider())
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapsGeocodeProvider"/> class.
        /// </summary>
        /// <param name="cache">
        /// The cache.
        /// </param>
        public GoogleMapsGeocodeProvider(IRuntimeCacheProvider cache) 
            : base(cache)
        {
        }

        /// <summary>
        /// Performs the Geocode request with Google Maps
        /// </summary>
        /// <param name="formattedAddress">
        /// The formatted address.
        /// </param>
        /// <returns>
        /// The <see cref="IGeocodeProviderResponse"/>.
        /// </returns>        
        protected override IGeocodeProviderResponse GetGeocodeProviderResponse(string formattedAddress)
        {
            var q = HttpUtility.UrlEncode(formattedAddress);

            var url = Settings.FirstOrDefault(x => x.Key == "urlString");

            if (string.IsNullOrEmpty(url.Value))
            {
                var ex = new Exception("The GoogleMaps Api Provider end point url was not set.");
                LogHelper.Error<GoogleMapsGeocodeProvider>("Endpoint could not be created", ex);

                throw ex;
            }

            var requestUriString = string.Format(url.Value, q);

            var req = (HttpWebRequest) WebRequest.Create(requestUriString);

            using (var resp = (HttpWebResponse) req.GetResponse())
            {
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    
                    var doc = XDocument.Parse(sr.ReadToEnd());

                    var status = doc.GetGeocodeStatus();
                    
                    return new GeocodeProviderResponse(status, status == GeocodeStatus.Ok ? doc.GetGeocodes() : new IGeocode[] { });

                }
            }
        }       

    }
}