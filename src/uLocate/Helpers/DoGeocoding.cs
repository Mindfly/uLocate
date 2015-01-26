namespace uLocate.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Xml.Linq;

    using uLocate.Configuration;
    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Logging;

    internal static class DoGeocoding
    {
        //TODO: Heather - change to use umbraco plugin manager to resolve the plugins rather than hard-coding this.

        internal static Coordinate GetCoordinateForAddress(IAddress TheAddress)
        {
            Coordinate returnCoordinate = new Coordinate();

            GoogleMapsGeocodeProvider mapProvider = new GoogleMapsGeocodeProvider(ApplicationContext.Current.ApplicationCache.RuntimeCache);

            var Response = mapProvider.Geocode(TheAddress);

            if (Response.Status == GeocodeStatus.Ok)
            {
                var Result = Response.Results.FirstOrDefault();
                if (Result != null)
                {
                    returnCoordinate.Latitude = Result.Latitude;
                    returnCoordinate.Longitude = Result.Longitude;
                }
            }

            return returnCoordinate;
        }
    }
}
