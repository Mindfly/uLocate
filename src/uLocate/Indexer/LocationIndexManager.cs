namespace uLocate.Indexer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    using Examine;
    using Examine.LuceneEngine;
    using Examine.Providers;

    using uLocate.Models;
    using uLocate.Services;

    using Umbraco.Core;
    using Umbraco.Core.Logging;

    internal class LocationIndexManager
    {
        public string IndexTypeName = "location";
        public string SearcherName = "uLocateLocationSearcher";
        public string IndexerName = "uLocateLocationIndexer";
        public string AllDataFieldName = "AllData";

        //private LocationService locationService = new LocationService(); < --- Cannot call this here - creates an infinite loop

        //private BaseIndexProvider uLocateLocationIndexer = ExamineManager.Instance.IndexProviderCollection[IndexerName];

        //private BaseSearchProvider uLocateLocationSearcher = ExamineManager.Instance.SearchProviderCollection[SearcherName];

        internal BaseSearchProvider uLocateLocationSearcher()
        {
            return ExamineManager.Instance.SearchProviderCollection[SearcherName];
        }

        internal BaseIndexProvider uLocateLocationIndexer()
        {
            return ExamineManager.Instance.IndexProviderCollection[IndexerName];
        }

        internal SimpleDataSet IndexLocation(EditableLocation Location, string IndexType, int IndexNodeId)
        {
            //LogHelper.Info<LocationIndexManager>(string.Format("IndexLocation for {0} (#{1}) STARTED", Location.Name, IndexNodeId));
            // create the node definition, ensure that it is the same type as referenced in the config
            var sds = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };
            sds.NodeDefinition.NodeId = IndexNodeId;
            sds.NodeDefinition.Type = IndexType;

            // add the data to the row (These are all listed in the ExamineIndex.config file)
            sds.RowData.Add("Key", Location.Key.ToString());
            sds.RowData.Add("Name", Location.Name);
            sds.RowData.Add("LocationTypeName", Location.LocationType.Name);
            sds.RowData.Add("LocationTypeKey", Location.LocationTypeKey.ToString());
            sds.RowData.Add("Latitude", Location.Latitude.ToString());
            sds.RowData.Add("Longitude", Location.Longitude.ToString());
            sds.RowData.Add("Address1", Location.Address.Address1);
            sds.RowData.Add("Address2", Location.Address.Address2);
            sds.RowData.Add("Locality", Location.Address.Locality);
            sds.RowData.Add("Region", Location.Address.Region);
            sds.RowData.Add("PostalCode", Location.Address.PostalCode);
            sds.RowData.Add("CountryCode", Location.Address.CountryCode);
            sds.RowData.Add("Email", Location.Email);
            sds.RowData.Add("Phone", Location.Phone);

            //We will add each custom property to the index, 
            //but in addition, all the custom data will be added in a blob - in case the <IndexUserFields> in ExamineIndex.config hasn't been updated with every custom property
            var allCustomPropData = new StringBuilder();

            foreach (var prop in Location.PropertyData)
            {
                if (prop.PropertyAttributes.IsDefaultProp == false)
                {
                    sds.RowData.Add(prop.PropertyAlias, prop.Value.ToString());
                    allCustomPropData.AppendFormat("{0}={1}={2}|", prop.Key, prop.PropertyAlias, prop.Value.ToString());
                }
            }

            sds.RowData.Add("CustomPropertyData", allCustomPropData.ToString());

            //Put all the data into a single field for simple keyword searches.
            var allData = new StringBuilder();

            foreach (var field in sds.RowData)
            {
                allData.AppendFormat("{0} | ", field.Value.ToString());
            }

            sds.RowData.Add(this.AllDataFieldName, allData.ToString());

            //LogHelper.Info<LocationIndexManager>(string.Format("DATA: {0}", sds.RowData.ToExamineXml(IndexNodeId, IndexType)));
            //LogHelper.Info<LocationIndexManager>(string.Format("IndexLocation for {0} (#{1}) COMPLETED", Location.Name, IndexNodeId));

            return sds;
        }

        internal StatusMessage RemoveLocation(EditableLocation Location, bool ReIndexIfNecessary = true)
        {
            var uLocateLocationIndexer = this.uLocateLocationIndexer(); //ExamineManager.Instance.IndexProviderCollection[this.IndexerName];
            var uLocateLocationSearcher = this.uLocateLocationSearcher(); //ExamineManager.Instance.SearchProviderCollection[this.SearcherName];

            var statusMsg = new StatusMessage();

            //search for the Key in the Index
            var searcher = uLocateLocationSearcher;
            var searchCriteria = searcher.CreateSearchCriteria();
            var query = searchCriteria.Field("Key", Location.Key.ToString());

            var searchResults = searcher.Search(query.Compile());

            var searchedLocations = uLocate.Helpers.Convert.ExamineToSearchedLocations(searchResults);
            var countMatches = searchedLocations.Count();

            if (countMatches == 0)
            {
                //No matches
                statusMsg.Success = true;
                statusMsg.Code = "NoMatch";
                statusMsg.Message = "No matching Location found in the index. Nothing to remove.";
            }
            else if (countMatches > 1)
            {
                //more than 1 match

                if (ReIndexIfNecessary)
                {
                    this.IndexAllLocations();
                    statusMsg = this.RemoveLocation(Location, false);
                }
                else
                {
                    statusMsg.Success = false;
                    statusMsg.Code = "DuplicateMatches";
                    statusMsg.Message = string.Format("{0} matching Locations were found in the index.", countMatches);
                }
            }
            else
            {
                //Exactly 1 match - Remove it
                var location = searchedLocations.Select(n => n.IndexedLocation).FirstOrDefault();
            
                statusMsg.ObjectName = location.Name;

                var examineId = location.IndexNodeId;
                if (examineId != 0)
                {
                    ExamineManager.Instance.DeleteFromIndex(examineId.ToString(), this.uLocateLocationIndexer().AsEnumerableOfOne());

                    statusMsg.Success = true;
                    statusMsg.Code = "MatchFoundAndDeleted";
                    statusMsg.Message = string.Format("The Location named '{0}' has been removed from the index. [Index #{1}]", location.Name, examineId);
                }
                else
                {
                    if (ReIndexIfNecessary)
                    {
                        this.IndexAllLocations();
                        statusMsg = this.RemoveLocation(Location, false);
                    }
                    else
                    {
                        statusMsg.Success = false;
                        statusMsg.Code = "MatchFoundNotDeleted";
                        statusMsg.Message =
                            string.Format(
                                "{0} Location was found, but has an id of {1} and has not been removed from the index.",
                                location.Name,
                                examineId);
                    }
                }
            }

            LogHelper.Info<LocationIndexManager>(statusMsg.Message);

            return statusMsg;
        }

        internal StatusMessage UpdateLocation(EditableLocation Location, bool ReIndexIfNecessary = true)
        {
            var uLocateLocationIndexer = this.uLocateLocationIndexer(); //ExamineManager.Instance.IndexProviderCollection[this.IndexerName];
            var uLocateLocationSearcher = this.uLocateLocationSearcher(); //ExamineManager.Instance.SearchProviderCollection[this.SearcherName];

            var statusMsg = new StatusMessage();

            //search for the Key in the Index
            var searcher = uLocateLocationSearcher;
            var searchCriteria = searcher.CreateSearchCriteria();
            var query = searchCriteria.Field("Key", Location.Key.ToString());

            var searchResults = searcher.Search(query.Compile());

            var searchedLocations = uLocate.Helpers.Convert.ExamineToSearchedLocations(searchResults);
            var countMatches = searchedLocations.Count();

            if (countMatches == 0)
            {
                //No matches, must be new

                //Add to index
                //var editableLocation = locationService.GetLocation(LocationKey).ConvertToEditableLocation();

                statusMsg.ObjectName = Location.Name;

                var examineId = this.GetMaxId(this.IndexTypeName) + 1;
                var sds = this.IndexLocation(Location, this.IndexTypeName, examineId);
                var examineNode = sds.RowData.ToExamineXml(examineId, this.IndexTypeName);
                ExamineManager.Instance.ReIndexNode(
                    examineNode,
                    this.IndexTypeName,
                    this.uLocateLocationIndexer().AsEnumerableOfOne());

                statusMsg.Success = true;
                statusMsg.Code = "AddedToIndex";
                statusMsg.Message = "No matching Location found in the index, added.";
            }
            else if (countMatches > 1)
            {
                //more than 1 match

                if (ReIndexIfNecessary)
                {
                    this.IndexAllLocations();
                    statusMsg = this.UpdateLocation(Location, false);

                    //Reindex all to clean up
                    //var childMsg = this.IndexAllLocations();
                    //statusMsg.Success = childMsg.Success;
                    //statusMsg.InnerStatuses.Add(childMsg);
                    //statusMsg.Code = "DuplicateMatches";
                    //if (childMsg.Success)
                    //{
                    //    statusMsg.Message = string.Format("{0} matching Locations were found in the index. All have been re-indexed.", countMatches);
                    //}
                    //else
                    //{
                    //    statusMsg.Message = string.Format("{0} matching Locations were found in the index. There was a problem re-indexing.", countMatches);
                    //}
                }
                else
                {
                    statusMsg.Code = "DuplicateMatches";
                    statusMsg.Message = string.Format("{0} matching Locations were found in the index. There was a problem re-indexing.", countMatches);
                }  
            }
            else
            {
                //Exactly 1 match
                var indexedLocation = searchedLocations.Select(n => n.IndexedLocation).FirstOrDefault();
                
                statusMsg.ObjectName = indexedLocation.Name;

                var examineId = indexedLocation.IndexNodeId;
                if (examineId != 0)
                {
                    ExamineManager.Instance.DeleteFromIndex(examineId.ToString(), this.uLocateLocationIndexer().AsEnumerableOfOne());

                    var sds = this.IndexLocation(Location, this.IndexTypeName, examineId);
                    var examineNode = sds.RowData.ToExamineXml(examineId, this.IndexTypeName);
                    ExamineManager.Instance.ReIndexNode(examineNode, this.IndexTypeName, this.uLocateLocationIndexer().AsEnumerableOfOne());

                    statusMsg.Success = true;
                    statusMsg.Code = "MatchFoundAndUpdated";
                    statusMsg.Message = string.Format("The Location named '{0}' has been removed from the index and re-added. [Index #{1}]", indexedLocation.Name, examineId);
                }
                else
                {
                    if (ReIndexIfNecessary)
                    {
                        this.IndexAllLocations();
                        statusMsg = this.UpdateLocation(Location, false);
                    }
                    else
                    {
                        //Reindex all to clean up
                        var childMsg = this.IndexAllLocations();
                        statusMsg.Success = childMsg.Success;
                        statusMsg.InnerStatuses.Add(childMsg);
                        statusMsg.Code = "MatchFoundDifferentId";
                        if (childMsg.Success)
                        {
                            statusMsg.Message =
                                string.Format(
                                    "{0} Location was found, but has an id of {1}. All locations have been re-indexed.",
                                    indexedLocation.Name,
                                    examineId);
                        }
                        else
                        {
                            statusMsg.Message =
                                string.Format(
                                    "{0} Location was found, but has an id of {1}. There was a problem re-indexing.",
                                    indexedLocation.Name,
                                    examineId);
                        }
                    }
                }
            }

            var secondaryMsg = statusMsg.InnerStatuses.Any() ? statusMsg.InnerStatuses[0].Message : "no inner message";
            LogHelper.Info<LocationIndexManager>(string.Format("{0} [{1}]", statusMsg.Message, secondaryMsg));

            return statusMsg;
        }

        internal StatusMessage IndexAllLocations()
        {
            var statusMsg = new StatusMessage();

            ExamineManager.Instance.IndexAll(this.IndexTypeName);

            statusMsg.Success = true;
            statusMsg.Message = "All Locations Indexed.";

            LogHelper.Info<LocationIndexManager>(statusMsg.Message);

            return statusMsg;
        }

        //void IndexAll(string type);
//bool IndexExists();
//void RebuildIndex();
//void ReIndexNode(XElement node, string type);
//void ReIndexNode(XElement node, string type, IEnumerable<BaseIndexProvider> providers);


        internal int GetMaxId(string IndexType)
        {
            LogHelper.Info<LocationIndexManager>(string.Format("GetMaxId for {0} STARTED", IndexType));

            // Get all existing items in the index of this type
            var searcher = this.uLocateLocationSearcher();
            var searchCriteria = searcher.CreateSearchCriteria("*");
            var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            if (searchResults.TotalItemCount != 0)
            {
                // Get the largest id
                var result = searchResults.Max(sr => sr.Id);
                LogHelper.Info<LocationIndexManager>(string.Format("GetMaxId for {0} = {1}", IndexType, result));

                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
