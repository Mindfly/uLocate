namespace uLocate.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using uLocate.Data;
    using uLocate.Models;

    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// The geography helper.
    /// </summary>
    public class GeographyHelper
    {
        const double MetricToMilesFactor = 1609.344;
        const int EarthSID = 4326;


        public static Sql GetGeoSearchSql(double SearchLat, double SearchLong, int MilesDistance, Guid FilterByLocationTypeKey)
        {
            var point = GetSqlPoint(SearchLat, SearchLong);

            var sqlSB = new StringBuilder();
            sqlSB.AppendLine(string.Format("WHERE [GeogCoordinate].STDistance(geography::{0})/{1} <= {2}", point, MetricToMilesFactor, MilesDistance));

            if (FilterByLocationTypeKey != Guid.Empty)
            {
                sqlSB.AppendLine(string.Format("AND [LocationTypeKey] = '{0}'", FilterByLocationTypeKey));
            }

            var sql = new Sql(sqlSB.ToString());
            return sql;
        }

        public static Sql GetGeoNearestSql(double SearchLat, double SearchLong, Guid FilterByLocationTypeKey)
        {
            var point = GetSqlPoint(SearchLat, SearchLong, true);

            var sqlSB = new StringBuilder();
            sqlSB.AppendLine(string.Format("WHERE [GeogCoordinate].STDistance('{0}') IS NOT NULL", point));

            if (FilterByLocationTypeKey != Guid.Empty)
            {
                sqlSB.AppendLine(string.Format("AND [LocationTypeKey] = '{0}'", FilterByLocationTypeKey));
            }

            sqlSB.AppendLine(string.Format("ORDER BY [GeogCoordinate].STDistance('{0}');", point));

            var sql = new Sql(sqlSB.ToString());
            return sql;
        }


        /// <summary>
        /// Gets the format for the POINT data used in an SQL query
        /// </summary>
        /// <param name="coordinate">
        /// The coordinate.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetSqlPoint(ICoordinate coordinate)
        {
            return string.Format("POINT ({0}, {1}, {2})", coordinate.Latitude, coordinate.Longitude, EarthSID);
        }

        /// <summary>
        /// Gets the format for the POINT data used in an SQL query
        /// </summary>
        /// <param name="Lat">
        /// The latitude value
        /// </param>
        /// <param name="Long">
        /// The longitude value
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetSqlPoint(double Lat, double Long, bool ExcludeCommas = false)
        {
            if (ExcludeCommas)
            {
                return string.Format("POINT({1} {0} {2})", Lat, Long, EarthSID);
            }
            else
            {
                return string.Format("Point({0}, {1}, {2})", Lat, Long, EarthSID);
            }
            
        }




        ///// <summary>
        ///// Gets a <see cref="ICoordinate"/> from the StPointText.
        ///// </summary>
        ///// <param name="text">
        ///// The text.
        ///// </param>
        ///// <returns>
        ///// The <see cref="ICoordinate"/>.
        ///// </returns>
        //public ICoordinate GetCoordinateFromStPointText(string text)
        //{
        //    return this.GetCoordinatesFromStText(text).FirstOrDefault();
        //}

        ///// <summary>
        ///// Gets a line string representation of the <see cref="IViewport"/>.
        ///// </summary>
        ///// <param name="viewport">
        ///// The viewport.
        ///// </param>
        ///// <returns>
        ///// The <see cref="string"/>.
        ///// </returns>
        //public string GetStLineString(Viewport viewport)
        //{
        //    return string.Format("LINESTRING ({0}, {1})", this.SplitLatLong(viewport.SouthWest), this.SplitLatLong(viewport.NorthEast));
        //}

        ///// <summary>
        ///// Gets a <see cref="IViewport"/> from the ST LINESTRING text.
        ///// </summary>
        ///// <param name="text">
        ///// The text.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IViewport"/>.
        ///// </returns>
        //public Viewport GetViewportFromStLinestringText(string text)
        //{
        //    var coordinates = this.GetCoordinatesFromStText(text).ToArray();

        //    if (2 != coordinates.Count()) return new Viewport();

        //    return new Viewport() { SouthWest = coordinates.First(), NorthEast = coordinates.Last() };
        //}

        ///// <summary>
        ///// Gets a collection of <see cref="ICoordinate"/>s from the ST text
        ///// </summary>
        ///// <param name="text">
        ///// The text.
        ///// </param>
        ///// <returns>
        ///// The <see cref="IEnumerable{ICoordinate}"/>.
        ///// </returns>
        //public IEnumerable<ICoordinate> GetCoordinatesFromStText(string text)
        //{
        //    var strip = text.Replace("POINT (", string.Empty)
        //        .Replace("LINESTRING (", string.Empty)
        //        .Replace(")", string.Empty);

        //    var pairs = strip.Split(',');

        //    try
        //    {
        //        return pairs.Select(pair => pair.Trim().Split(' ')).Select(lngLat => new Coordinate() { Longitude = double.Parse(lngLat[0]), Latitude = double.Parse(lngLat[1]) });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error<GeographyHelper>("Failed to parse ST TEXT", ex);
        //        return new ICoordinate[] { };
        //    }
        //}

        ///// <summary>
        ///// Splits the coordinate into it's latitude and longitude values.
        ///// </summary>
        ///// <param name="coordinate">
        ///// The coordinate.
        ///// </param>
        ///// <returns>
        ///// The <see cref="string"/>.
        ///// </returns>
        //private string SplitLatLong(ICoordinate coordinate)
        //{
        //    var valid = coordinate ?? new Coordinate() { Latitude = 0, Longitude = 0 };
            
        //    return string.Format("{0} {1}", valid.Longitude, valid.Latitude);
        //}
    }
}