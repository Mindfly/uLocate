namespace uLocate.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    using Examine;
    using Examine.LuceneEngine;

    using uLocate.Models;
    using uLocate.Services;

    public static class Convert
    {
        private static LocationTypeService locationTypeService = new LocationTypeService();

        internal static IEnumerable<JsonLocation> LocationsToJsonLocations(IEnumerable<Location> Locations)
        {
            var ReturnList = new List<JsonLocation>();

            foreach (var loc in Locations)
            {
                ReturnList.Add(new JsonLocation(loc));
            }

            return ReturnList;
        }

        internal static IEnumerable<Location> JsonLocationsToLocations(IEnumerable<JsonLocation> JsonLocations)
        {
            var ReturnList = new List<Location>();

            foreach (var loc in JsonLocations)
            {
                ReturnList.Add(loc.ConvertToLocation());
            }

            return ReturnList;
        }

        public static IEnumerable<SearchedLocation> ExamineToSearchedLocations(ISearchResults SearchResults)
        {
            var returnList = new List<SearchedLocation>();

            // check that there are any search results
            if (SearchResults.TotalItemCount > 0)
            {
                // iterate through the search results
                foreach (SearchResult result in SearchResults)
                {
                    var sl = new SearchedLocation();
                    sl.ExamineNodeId = result.Id;
                    sl.SearchScore = result.Score;

                    var location = new JsonLocation();
                    location.Key = result.Fields.ContainsKey("Key") ? new Guid(result.Fields["Key"]) : Guid.Empty;
                    location.Name = result.Fields.ContainsKey("Name") ? result.Fields["Name"] : string.Empty;
                    location.LocationTypeKey = result.Fields.ContainsKey("LocationTypeKey") ? new Guid(result.Fields["LocationTypeKey"]) : Guid.Empty;
                    location.Latitude = result.Fields.ContainsKey("Latitude") ? System.Convert.ToDouble(result.Fields["Latitude"]) : 0;
                    location.Longitude = result.Fields.ContainsKey("Longitude") ? System.Convert.ToDouble(result.Fields["Longitude"]) : 0;
                    location.Address1 = result.Fields.ContainsKey("Address1") ? result.Fields["Address1"] : string.Empty;
                    location.Address2 = result.Fields.ContainsKey("Address2") ? result.Fields["Address2"] : string.Empty;
                    location.Locality = result.Fields.ContainsKey("Locality") ? result.Fields["Locality"] : string.Empty;
                    location.Region = result.Fields.ContainsKey("Region") ? result.Fields["Region"] : string.Empty;
                    location.PostalCode = result.Fields.ContainsKey("PostalCode") ? result.Fields["PostalCode"] : string.Empty;
                    location.CountryCode = result.Fields.ContainsKey("CountryCode") ? result.Fields["CountryCode"] : string.Empty;
                    location.Email = result.Fields.ContainsKey("Email") ? result.Fields["Email"] : string.Empty;
                    location.Phone = result.Fields.ContainsKey("Phone") ? result.Fields["Phone"] : string.Empty;

                    location.CustomPropertyData = result.Fields.ContainsKey("CustomProperties") ? ExamineCustomPropsToJsonProps(result.Fields["CustomProperties"]) : new List<JsonPropertyData>();

                    // add the location to SL
                    sl.JsonLocation = location;

                    //add to list
                    returnList.Add(sl);
                }
            }

            return returnList;
        }

        private static List<JsonPropertyData> ExamineCustomPropsToJsonProps(string PropsString)
        {
            var returnList = new List<JsonPropertyData>();

            var dict = ParseCustomPropsToDict(PropsString);

            foreach (var kv in dict)
            {
                var jsonProp = new JsonPropertyData();
                jsonProp.PropAlias = kv.Key;
                jsonProp.PropData = kv.Value;
            }

            return returnList;
        }

        private static Dictionary<string, string> ParseCustomPropsToDict(string PropsString)
        {
            var returnDict = new Dictionary<string, string>();

            var pairs = PropsString.Split('|');

            foreach (var pair in pairs)
            {
                var kv = pair.Split('=');
                returnDict.Add(kv[0], kv[1]);
            }

            return returnDict;
        }



    }
}
