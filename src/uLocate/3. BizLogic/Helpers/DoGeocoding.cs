namespace uLocate.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Remoting.Messaging;
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

        internal static GeocodeStatus GetGeocodeStatus(string GeoStatusString)
        {
            switch (GeoStatusString)
            {
                case "Ok":
                    return GeocodeStatus.Ok;
                    break;
                case "ZeroResults":
                    return GeocodeStatus.ZeroResults;
                    break;
                case "RequestDenied":
                    return GeocodeStatus.RequestDenied;
                    break;
                case "InvalidRequest":
                    return GeocodeStatus.InvalidRequest;
                    break;
                case "NotQueried":
                    return GeocodeStatus.NotQueried;
                    break;
                case "OverQueryLimit":
                    return GeocodeStatus.OverQueryLimit;
                    break;
                case "Error":
                    return GeocodeStatus.Error;
                    break;
                default:
                    return GeocodeStatus.NotQueried;
                    break;
            }
        }
    }
}
