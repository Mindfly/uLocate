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
    public class CountryHelper
    {
        /// <summary>
        /// Get all possible countries from CultureInfo
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{ICountry}"/>.
        /// </returns>
        public static IEnumerable<Country> GetAllCountries()
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