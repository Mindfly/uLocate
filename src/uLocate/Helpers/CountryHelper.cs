namespace uLocate.Helpers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using uLocate.Models;
    using Umbraco.Core;

    /// <summary>
    /// The country helper.
    /// </summary>
    internal class CountryHelper
    {
        /// <summary>
        /// The get all countries.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{ICountry}"/>.
        /// </returns>
        public static IEnumerable<ICountry> GetAllCountries()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(culture => new RegionInfo(culture.Name))
                .Select(ri => new Country()
                                  {
                                      CountryCode = ri.TwoLetterISORegionName,
                                      Name = ri.EnglishName
                                  }).DistinctBy(x => x.CountryCode).OrderBy(x => x.Name);
        }
    }
}