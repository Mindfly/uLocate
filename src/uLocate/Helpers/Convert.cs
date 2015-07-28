namespace uLocate.Helpers
{
    using System;
    using System.Collections.Generic;

    using Examine;

    using uLocate.Models;
    using uLocate.Services;

    public static class Convert
    {
        private static LocationTypeService locationTypeService = new LocationTypeService();

        internal static IEnumerable<IndexedLocation> EditableLocationsToIndexedLocations(IEnumerable<EditableLocation> Locations)
        {
            var ReturnList = new List<IndexedLocation>();

            foreach (var loc in Locations)
            {
                ReturnList.Add(new IndexedLocation(loc));
            }

            return ReturnList;
        }

        internal static IndexedLocation EditableLocationToIndexedLocation(EditableLocation Location)
        {
            return new IndexedLocation(Location);
        }

        internal static IEnumerable<EditableLocation> IndexedLocationsToEditableLocations(IEnumerable<IndexedLocation> JsonLocations)
        {
            var ReturnList = new List<EditableLocation>();

            foreach (var loc in JsonLocations)
            {
                ReturnList.Add(loc.ConvertToEditableLocation());
            }

            return ReturnList;
        }

        internal static IEnumerable<SearchedLocation> ExamineToSearchedLocations(ISearchResults SearchResults)
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

                    var location = new IndexedLocation();
                    location.IndexNodeId = result.Id;
                    location.Key = result.Fields.ContainsKey("Key") ? new Guid(result.Fields["Key"]) : Guid.Empty;
                    location.Name = result.Fields.ContainsKey("Name") ? result.Fields["Name"] : string.Empty;
                    location.LocationTypeKey = result.Fields.ContainsKey("LocationTypeKey") ? new Guid(result.Fields["LocationTypeKey"]) : Guid.Empty;
                    location.LocationTypeName = result.Fields.ContainsKey("LocationTypeName") ? result.Fields["LocationTypeName"] : string.Empty;
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

                    //Handle Custom properties
                    List<IndexedPropertyData> customProps;

                    if (result.Fields.ContainsKey("CustomPropertyData"))
                    {
                        customProps = ExamineCustomPropsToIndexedProps(result.Fields["CustomPropertyData"]);
                    }
                    else
                    {
                        customProps = new List<IndexedPropertyData>();
                    }

                    location.CustomPropertyData = customProps;

                    //Add all properties to Dictionary
                    location.AllPropertiesDictionary = AllPropertiesToDictionary(result.Fields, customProps);

                    // add the location to SL
                    sl.IndexedLocation = location;

                    //add to list
                    returnList.Add(sl);
                }
            }

            return returnList;
        }


        #region Private

        private static Dictionary<string, string> AllPropertiesToDictionary(IDictionary<string, string> ResultFields, List<IndexedPropertyData> CustomProperties)
        {
            var returnDict = new Dictionary<string, string>();


            if (ResultFields.ContainsKey("Key"))
            {
                returnDict.Add("Key", ResultFields["Key"]);
            }

            if (ResultFields.ContainsKey("Name"))
            {
                returnDict.Add("Name", ResultFields["Name"]);
            }

            if (ResultFields.ContainsKey("LocationTypeKey"))
            {
                returnDict.Add("LocationTypeKey", ResultFields["LocationTypeKey"]);
            }

            if (ResultFields.ContainsKey("Latitude"))
            {
                returnDict.Add("Latitude", ResultFields["Latitude"]);
            }

            if (ResultFields.ContainsKey("Longitude"))
            {
                returnDict.Add("Longitude", ResultFields["Longitude"]);
            }

            if (ResultFields.ContainsKey("Address1"))
            {
                returnDict.Add("Address1", ResultFields["Address1"]);
            }

            if (ResultFields.ContainsKey("Address2"))
            {
                returnDict.Add("Address2", ResultFields["Address2"]);
            }

            if (ResultFields.ContainsKey("Locality"))
            {
                returnDict.Add("Locality", ResultFields["Locality"]);
            }

            if (ResultFields.ContainsKey("Region"))
            {
                returnDict.Add("Region", ResultFields["Region"]);
            }

            if (ResultFields.ContainsKey("PostalCode"))
            {
                returnDict.Add("PostalCode", ResultFields["PostalCode"]);
            }

            if (ResultFields.ContainsKey("CountryCode"))
            {
                returnDict.Add("CountryCode", ResultFields["CountryCode"]);
            }

            if (ResultFields.ContainsKey("Email"))
            {
                returnDict.Add("Email", ResultFields["Email"]);
            }

            if (ResultFields.ContainsKey("Phone"))
            {
                returnDict.Add("Phone", ResultFields["Phone"]);
            }

            //Handle Custom properties
            if (CustomProperties != null)
            {
                foreach (var prop in CustomProperties)
                {
                    returnDict.Add(prop.PropAlias, prop.PropData.ToString());
                } 
            }
            else if (ResultFields.ContainsKey("CustomPropertyData"))
            {
                var props = ExamineCustomPropsToIndexedProps(ResultFields["CustomPropertyData"]);

                foreach (var prop in props)
                {
                    returnDict.Add(prop.PropAlias, prop.PropData.ToString());
                } 
            }

            return returnDict;
        }

        private static List<IndexedPropertyData> ExamineCustomPropsToIndexedProps(string PropsString)
        {
            var returnList = new List<IndexedPropertyData>();

            //var dict = ParseCustomPropsToDict(PropsString);

            var pairs = PropsString.Trim().Split('|');

            foreach (var pair in pairs)
            {
                if (pair != "")
                {
                    var kav = pair.Split('=');

                    var jsonProp = new IndexedPropertyData();
                    jsonProp.Key = new Guid(kav[0]);
                    jsonProp.PropAlias = kav[1];
                    jsonProp.PropData = kav[2];
                    returnList.Add(jsonProp);
                }
            }

            return returnList;
        }

        private static Dictionary<string, string> ParseCustomPropsToDict(string PropsString)
        {
            var returnDict = new Dictionary<string, string>();

            var pairs = PropsString.Trim().Split('|');

            foreach (var pair in pairs)
            {
                if (pair != "")
                {
                    var kv = pair.Split('=');
                    returnDict.Add(kv[0], kv[1]);
                }
            }

            return returnDict;
        }

        #endregion
    }
}
