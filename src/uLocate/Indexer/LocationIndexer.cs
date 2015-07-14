namespace uLocate.Indexer
{
    using System.Collections.Generic;
    using System.Text;

    using Examine;
    using Examine.LuceneEngine;

    using uLocate.Models;
    using uLocate.Persistance;

    public class LocationIndexer : Examine.LuceneEngine.ISimpleDataService
    {
        //private string IndexNodeTypeName = "ulocatelocationdata";

        private LocationIndexManager locationIndexManager = new LocationIndexManager();
        
        public IEnumerable<SimpleDataSet> GetAllData(string indexType)
        {
            var uLocateLocationIndexer = ExamineManager.Instance.IndexProviderCollection[locationIndexManager.IndexerName];

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

            return indexData;
          }
    }
}
