namespace uLocate.Plugins.Geocode.GoogleMaps
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Models;

    /// <summary>
    /// Extension methods for the XDocument and the Google Maps API response
    /// </summary>
    internal static class GoogleResposneXDocumentExtensions
    {
        /// <summary>
        /// Gets a collection of <see cref="IGeocode"/> from the xml response data
        /// </summary>
        /// <param name="doc">
        /// The doc.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{IGeocode}"/>.
        /// </returns>
        public static IEnumerable<IGeocode> GetGeocodes(this XDocument doc)
        {
            var results = doc.Descendants().Where(x => x.Name.LocalName == "result").ToArray();

            if (!results.Any()) return new IGeocode[] { };

            return results.Select(res => new Models.Geocode()
            {
                Latitude = res.GetSafeElementValueAsDouble("lat"),
                Longitude = res.GetSafeElementValueAsDouble("lng"),
                FormattedAddress = res.GetSafeElementValue("formatted_address"),
                Quality = res.GetGeocodeQuality(),
                Viewport = res.GetViewport()
            });
        }

        /// <summary>
        /// Gets a <see cref="Viewport"/> for a single result
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="Viewport"/>.
        /// </returns>
        public static Viewport GetViewport(this XElement result)
        {
            var southwest = result.Descendants("southwest").FirstOrDefault();
            var northeast = result.Descendants("northeast").FirstOrDefault();

            return southwest == null || northeast == null
                ? new Viewport()
                : new Viewport()
                {
                    SouthWest = new Coordinate()
                    {
                        Latitude = southwest.GetSafeElementValueAsDouble("lat"), 
                        Longitude = southwest.GetSafeElementValueAsDouble("lng")
                    },
                    NorthEast = new Coordinate()
                    {
                        Latitude = northeast.GetSafeElementValueAsDouble("lat"),
                        Longitude = northeast.GetSafeElementValueAsDouble("lng")
                    }
                };
        }


        /// <summary>
        /// Gets the <see cref="GeocodeQuality"/> for a single result
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="GeocodeQuality"/>.
        /// </returns>
        public static GeocodeQuality GetGeocodeQuality(this XElement result)
        {
            var geometry = result.Element("geometry");

            if (geometry == null) return GeocodeQuality.None;

            var locationType = geometry.GetSafeElementValue("location_type");

            switch (locationType)
            {
                case "ROOFTOP":
                    return GeocodeQuality.Rooftop;

                case "RANGE_INTERPOLATED":
                    return GeocodeQuality.RangeInterpolated;

                case "GEOMETRIC_CENTER":
                    return GeocodeQuality.Center;

                case "APPROXIMATE":
                    return GeocodeQuality.Approximate;

                default:
                    return GeocodeQuality.None;
            }
        }

        /// <summary>
        /// Gets the <see cref="GeocodeStatus"/>
        /// </summary>
        /// <param name="doc">
        /// The doc.
        /// </param>
        /// <returns>
        /// The <see cref="GeocodeStatus"/>.
        /// </returns>
        public static GeocodeStatus GetGeocodeStatus(this XDocument doc)
        {
            var el = doc.Descendants("status").FirstOrDefault();

            if (el == null) return GeocodeStatus.Error;

            switch (el.Value)
            {
                case "ZERO_RESULTS":
                    return GeocodeStatus.ZeroResults;

                case "OVER_QUERY_LIMIT":
                    return GeocodeStatus.OverQueryLimit;

                case "REQUEST_DENIED":
                    return GeocodeStatus.RequestDenied;

                case "INVALID_REQUEST":
                    return GeocodeStatus.InvalidRequest;

                case "UNKNOWN_ERROR":
                    return GeocodeStatus.Error;

                case "NOT_QUERIED":
                    return GeocodeStatus.NotQueried;

                default:
                    return GeocodeStatus.Ok;
            }
        }

        /// <summary>
        /// The get safe element value.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSafeElementValue(this XElement element, string localName)
        {
            var target = element.Descendants().FirstOrDefault(x => x.Name.LocalName == localName);

            return target == null ? string.Empty : target.Value.Trim();
        }

        /// <summary>
        /// The get safe attribute value.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="elementLocalName">
        /// The element local name.
        /// </param>
        /// <param name="attributeLocalName">
        /// The attribute local name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSafeAttributeValue(this XElement element, string elementLocalName, string attributeLocalName)
        {
            var target = element.Descendants().FirstOrDefault(x => x.Name.LocalName == elementLocalName);

            if (target == null) return string.Empty;

            var attribute = target.Attributes().FirstOrDefault(x => x.Name.LocalName == attributeLocalName);

            return attribute == null ? string.Empty : attribute.Value.Trim();
        }

        /// <summary>
        /// The get safe element value as double.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double GetSafeElementValueAsDouble(this XElement element, string localName)
        {
            var value = element.GetSafeElementValue(localName);

            double db;

            return double.TryParse(value, out db) ? db : 0;
        }
    }
}