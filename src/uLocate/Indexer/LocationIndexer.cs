namespace uLocate.Indexer
{
    using System.Collections.Generic;

    using Examine;
    using Examine.LuceneEngine;

    using uLocate.Persistance;

    using Umbraco.Core.Logging;

    public class LocationIndexer : Examine.LuceneEngine.ISimpleDataService
    {       
        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            LogHelper.Debug<LocationIndexer>("GetAllData STARTED");
            LocationIndexManager locationIndexManager = new LocationIndexManager();

            //var uLocateLocationIndexer = ExamineManager.Instance.IndexProviderCollection[locationIndexManager.IndexerName];

            var counterId = locationIndexManager.GetMaxId(indexType) + 1;

            var indexData = new List<SimpleDataSet>();

            var allLocations = Repositories.LocationRepo.GetAll();

            //iterate the locations, adding a SimpleDataSet to the Index for each
            foreach (var location in allLocations)
            {
                var sds = locationIndexManager.IndexLocation(location, indexType, counterId);

                indexData.Add(sds);
                counterId++;

                //yield return sds;
            }

            LogHelper.Debug<LocationIndexer>("GetAllData COMPLETE");

            return indexData;
          }
    }
}
