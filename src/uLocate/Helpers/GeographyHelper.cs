namespace uLocate.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using uLocate.Models;

    using Umbraco.Core.Logging;

    /// <summary>
    /// The geography helper.
    /// </summary>
    internal class GeographyHelper
    {
        /// <summary>
        /// The get st point text.
        /// </summary>
        /// <param name="coordinate">
        /// The coordinate.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetStPointText(ICoordinate coordinate)
        {
            return string.Format("POINT ({0})", this.SplitLatLong(coordinate));
        }

        /// <summary>
        /// Gets a <see cref="ICoordinate"/> from the StPointText.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="ICoordinate"/>.
        /// </returns>
        public ICoordinate GetCoordinateFromStPointText(string text)
        {
            return this.GetCoordinatesFromStText(text).FirstOrDefault();
        }

        /// <summary>
        /// Gets a line string representation of the <see cref="IViewport"/>.
        /// </summary>
        /// <param name="viewport">
        /// The viewport.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetStLineString(IViewport viewport)
        {
            return string.Format("LINESTRING ({0}, {1})", this.SplitLatLong(viewport.SouthWest), this.SplitLatLong(viewport.NorthEast));
        }

        /// <summary>
        /// Gets a <see cref="IViewport"/> from the ST LINESTRING text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="IViewport"/>.
        /// </returns>
        public IViewport GetViewportFromStLinestringText(string text)
        {
            var coordinates = this.GetCoordinatesFromStText(text).ToArray();

            if (2 != coordinates.Count()) return new Viewport();

            return new Viewport() { SouthWest = coordinates.First(), NorthEast = coordinates.Last() };
        }

        /// <summary>
        /// Gets a collection of <see cref="ICoordinate"/>s from the ST text
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{ICoordinate}"/>.
        /// </returns>
        public IEnumerable<ICoordinate> GetCoordinatesFromStText(string text)
        {
            var strip = text.Replace("POINT (", string.Empty)
                .Replace("LINESTRING (", string.Empty)
                .Replace(")", string.Empty);

            var pairs = strip.Split(',');

            try
            {
                return pairs.Select(pair => pair.Trim().Split(' ')).Select(lngLat => new Coordinate() { Longitude = double.Parse(lngLat[0]), Latitude = double.Parse(lngLat[1]) });
            }
            catch (Exception ex)
            {
                LogHelper.Error<GeographyHelper>("Failed to parse ST TEXT", ex);
                return new ICoordinate[] { };
            }
        }

        /// <summary>
        /// Splits the coordinate into it's latitude and longitude values.
        /// </summary>
        /// <param name="coordinate">
        /// The coordinate.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string SplitLatLong(ICoordinate coordinate)
        {
            var valid = coordinate ?? new Coordinate() { Latitude = 0, Longitude = 0 };
            
            return string.Format("{0} {1}", valid.Longitude, valid.Latitude);
        }
    }
}