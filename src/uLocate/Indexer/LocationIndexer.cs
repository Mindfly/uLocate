namespace uLocate.Indexer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Examine;
    using Examine.LuceneEngine;

    using uLocate.Persistance;

    public class LocationIndexer : Examine.LuceneEngine.ISimpleDataService
    {
        //private string IndexNodeTypeName = "ulocatelocationdata";

        private string SearcherName = "uLocateLocationSearcher";

        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            var counterId = GetMaxId(indexType) + 1;

            var indexData = new List<SimpleDataSet>();

            var allLocations = Repositories.LocationRepo.GetAll();

            //iterate the location data
            foreach (var location in allLocations)
            {
                // create the node definition, ensure that it is the same type as referenced in the config
                var sds = new SimpleDataSet { NodeDefinition = new IndexedNode(), RowData = new Dictionary<string, string>() };
                sds.NodeDefinition.NodeId = counterId;
                sds.NodeDefinition.Type = indexType;
            
                // add the data to the row
                sds.RowData.Add("Key", location.Key.ToString());
                sds.RowData.Add("Name", location.Name);
                sds.RowData.Add("LocationTypeName", location.LocationType.Name);
                sds.RowData.Add("LocationTypeKey", location.LocationTypeKey.ToString());
                sds.RowData.Add("Latitude", location.Latitude.ToString());
                sds.RowData.Add("Longitude", location.Longitude.ToString());
                sds.RowData.Add("Address1", location.Address.Address1);
                sds.RowData.Add("Address2", location.Address.Address2);
                sds.RowData.Add("Locality", location.Address.Locality);
                sds.RowData.Add("Region", location.Address.Region);
                sds.RowData.Add("PostalCode", location.Address.PostalCode);
                sds.RowData.Add("CountryCode", location.Address.CountryCode);
                sds.RowData.Add("Email", location.Email);
                sds.RowData.Add("Phone", location.Phone);

                //We will add each custom property to the index, 
                //but in addition, all the custom data will be added in a blob - in case the <IndexUserFields> in ExamineIndex.config hasn't been updated with every custom property
                var allCustomPropData = new StringBuilder();

                foreach (var prop in location.PropertyData)
                {
                    if (prop.PropertyAttributes.IsDefaultProp == false)
                    {
                        sds.RowData.Add(prop.PropertyAlias, prop.Value.ToString());
                        allCustomPropData.AppendFormat(
                            "{0}={1}={2}|",
                            prop.Key,
                            prop.PropertyAlias,
                            prop.Value.ToString());
                    }
                }

                sds.RowData.Add("CustomPropertyData", allCustomPropData.ToString());

                indexData.Add(sds);
                counterId++;

                //yield return sds;
            }

            return indexData;
          }

        private int GetMaxId(string indexType)
        {
            // Get all existing items in the index of this type
            var searcher = ExamineManager.Instance.SearchProviderCollection[SearcherName];
            var searchResults = searcher.Search(
                searcher.CreateSearchCriteria(indexType).SearchIndexType,
                true
                );

            // Get the largest id
            if (searchResults.TotalItemCount != 0)
            {
                return searchResults.Max(sr => sr.Id);
            }
            else
            {
                return 0;
            }
        }
        
    }
}
