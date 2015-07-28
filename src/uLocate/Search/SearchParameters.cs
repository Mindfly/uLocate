namespace uLocate.Search
{
    using System;
    using System.Collections.Generic;

    using uLocate.Indexer;

    /// <summary>
    /// All the parameters needed by the Search class.
    /// </summary>
    public class SearchParameters
    {
        /// <summary>
        /// Search term, unescaped, as input by user
        /// </summary>
        public string SearchTerm { get; set; }
       
        /// <summary>
        /// Only show results of these LocationTypes
        /// </summary>
        public List<string> LocationTypes { get; set; }

        /// <summary>
        /// Search provider name
        /// </summary>
        public string SearchProvider { get; internal set; }
        //{
        //    get
        //    {
        //        LocationIndexManager locationIndexManager = new LocationIndexManager();
        //        return locationIndexManager.SearcherName;
        //    }
        //}
        
        /// <summary>
        /// List of properties to search, boost/fuzzy values to assign, 
        /// </summary>
        public List<SearchProperty> SearchProperties { get; set; }
        
        ///// <summary>
        ///// Types of content to search
        ///// </summary>
        public List<string> IndexTypes { get; internal set; }

        //Set some default values
        public SearchParameters()
        {
            LocationIndexManager locationIndexManager = new LocationIndexManager();
            this.SearchProperties = new List<SearchProperty> { new SearchProperty(locationIndexManager.AllDataFieldName) };
            this.IndexTypes = new List<string> { locationIndexManager.IndexTypeName };
            this.SearchProvider = locationIndexManager.uLocateLocationSearcher().Name;
        }

        //string GetSearchProvider()
        //{
        //    var searchProvider = Config.Instance.GetByKey("SearchProvider");
        //    if (string.IsNullOrEmpty(searchProvider))
        //        throw new ArgumentException("SearchProvider must be set in FullTextSearch.Config");
        //    return searchProvider;
        //}
    }
}