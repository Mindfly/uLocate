namespace uLocate.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    using ClientDependency.Core;

    using Examine;

    using Excel;

    using uLocate.Indexer;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Core.Cache;

    using UmbracoExamine;

    /// <summary>
    /// Functions related to looking up and editing data which can be called from Api Controllers etc.
    /// </summary>
    public class LocationService
    {
        private ICacheProvider _requestCache = ApplicationContext.Current.ApplicationCache.RequestCache;

        private LocationTypeService locTypeService = new LocationTypeService();

        private LocationIndexManager locationIndexManager = new LocationIndexManager();

        //private string IndexNodeTypeName = locationIndexManager.IndexTypeName;

        //public LocationService()
        //{
        //    _requestCache = ApplicationContext.Current.ApplicationCache.RequestCache;
        //}

        #region CUD Operations

        public EditableLocation CreateLocation(string LocationName, bool UpdateIfFound = false)
        {
            var result = CreateLocation(LocationName, uLocate.Constants.DefaultLocationTypeKey, UpdateIfFound);

            return result;
        }

        public EditableLocation CreateLocation(string LocationName, Guid LocationTypeGuid, bool UpdateIfFound = false)
        {
            bool DoUpdate = false;
            Guid locTypeKey;

            if (LocationTypeGuid != Guid.Empty)
            {
                locTypeKey = LocationTypeGuid;
            }
            else
            {
                locTypeKey = uLocate.Constants.DefaultLocationTypeKey;
            }

            if (UpdateIfFound)
            {
                //Lookup first
                var matchingLocations = Repositories.LocationRepo.GetByName(LocationName);
                if (matchingLocations.Any())
                {
                    EditableLocation lookupLoc = matchingLocations.FirstOrDefault();
                    lookupLoc.Name = LocationName;
                    lookupLoc.LocationTypeKey = locTypeKey;
                    Repositories.LocationRepo.Update(lookupLoc);
                    locationIndexManager.UpdateLocation(lookupLoc.Key);
                    return lookupLoc;
                }
            }

            EditableLocation newLoc = new EditableLocation(LocationName, locTypeKey);
            Repositories.LocationRepo.Insert(newLoc);
            locationIndexManager.UpdateLocation(newLoc.Key);
            return newLoc;
        }


        public IndexedLocation UpdateLocation(IndexedLocation UpdatedIndexedLocation)
        {
            var key = UpdatedIndexedLocation.Key;

            var entity = UpdatedIndexedLocation.ConvertToEditableLocation();
            Repositories.LocationRepo.Update(entity);

            var result = Repositories.LocationRepo.GetByKey(key);
            ////uLocate.Helpers.Persistence.UpdateLocation();

            return new IndexedLocation(result);
        }

        public EditableLocation UpdateLocation(EditableLocation UpdatedEditableLocation)
        {
            Repositories.LocationRepo.Update(UpdatedEditableLocation);

            var Result = Repositories.LocationRepo.GetByKey(UpdatedEditableLocation.Key);

            return Result;
        }

        public StatusMessage UpdateGeographyData(EditableLocation EditableLocationToUpdate)
        {
            var returnMsg = new StatusMessage();

            try
            {
                Repositories.LocationRepo.UpdateLatLong(EditableLocationToUpdate);
            }
            catch (Exception exLatLong)
            {
                returnMsg.Success = false;
                returnMsg.Code = "ErrorGeoCode";
                returnMsg.Message = string.Format("{0} location was unable to be updated. Error while geo-coding.", EditableLocationToUpdate.Name);
                returnMsg.RelatedException = exLatLong;
            }

            try
            {
                Repositories.LocationRepo.UpdateDbGeography(EditableLocationToUpdate);
            }
            catch (Exception eDb)
            {
                returnMsg.Success = false;
                returnMsg.Code = "ErrorDbGeography";
                returnMsg.Message = string.Format("{0} location was unable to be updated. Error while updating database geography.", EditableLocationToUpdate.Name);
                returnMsg.RelatedException = eDb;
            }

            returnMsg.Success = true;
            returnMsg.Message = string.Format("{0} location was updated.", EditableLocationToUpdate.Name);

            return returnMsg;
        }

        public StatusMessage UpdateCoordinatesAsNeeded()
        {
            var Result = Repositories.LocationRepo.UpdateGeoForAllNeeded();

            return Result;
        }
        
        public StatusMessage DeleteLocation(Guid LocationKey)
        {
            var Result = Repositories.LocationRepo.Delete(LocationKey);

            return Result;
        }

        #endregion

        #region Location Querying

        public StatusMessage CountLocations<TKey>(Guid LocationTypeKey, Func<IndexedLocation, TKey> GroupingProperty)
        {
            //TODO: Check if a direct search would be faster
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var locTypeName = locTypeService.GetLocationType(LocationTypeKey).Name;

            var allLocations = this.GetLocations(LocationTypeKey);

            var query = (from loc in allLocations select loc).GroupBy(GroupingProperty);

            var iTotal = 0;
            var msg = new StatusMessage();


            foreach (var nameGroup in query)
            {
                var thisGroupMsg = new StatusMessage();
                var iGroupCount = 0;

                foreach (var loc in nameGroup)
                {
                    iGroupCount++;
                    iTotal++;
                }

                thisGroupMsg.Message = string.Format("{0} locations in the group {1}", iGroupCount, nameGroup.Key);

                msg.InnerStatuses.Add(thisGroupMsg);
            }

            msg.Message = string.Format("Total of {0} locations of type '{1}'", iTotal, locTypeName);
            return msg;
        }

        public IndexedLocation GetLocation(Guid LocationKey)
        {
            //TODO: Check if a direct search would be faster than using the Repo or LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);
            
            var result = this.GetLocations().Where(l => l.Key == LocationKey).FirstOrDefault();
            //var result = Repositories.LocationRepo.GetByKey(LocationKey);

            return result;
        }
        
        public IEnumerable<IndexedLocation> GetLocations(Guid LocationTypeKey)
        {
            //OLD
            //var result = (IEnumerable<EditableLocation>)_requestCache.GetCacheItem("all-locations-by-type", () => Repositories.LocationRepo.GetByType(LocationTypeKey));
            //var filteredLocs = Repositories.LocationRepo.GetByType(locTypeKey);
            //var result = uLocate.Helpers.Convert.LocationsToJsonLocations(filteredLocs);

            //TODO: Check if a direct search would be faster than LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var result = this.GetLocations().Where(l => l.LocationTypeKey == LocationTypeKey);

            return result;
        }

        public IEnumerable<IndexedLocation> GetLocations(string LocationName)
        {
            //OLD
            //var matchingLocations = Repositories.LocationRepo.GetByName(LocName);
            //var result = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(matchingLocations);

            //TODO: Check if a direct search would be faster than using the Repo or LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var result = this.GetLocations().Where(l => l.Name == LocationName);
            
            return result;
        }

        public IEnumerable<IndexedLocation> GetLocations()
        {
            //OLD
            //var result = (IEnumerable<EditableLocation>)_requestCache.GetCacheItem("all-locations", () => Repositories.LocationRepo.GetAll());
            //var result = Repositories.LocationRepo.GetAll();

            var searcher = locationIndexManager.uLocateLocationSearcher();
            var searchCriteria = searcher.CreateSearchCriteria("*");
            var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var searchedLocations = uLocate.Helpers.Convert.ExamineToSearchedLocations(searchResults);

            var result = searchedLocations.Select(n => n.IndexedLocation);
            return result;
        }

        public IEnumerable<IndexedLocation> GetLocationsByPropertyValue(string PropertyAlias, string Value)
        {
            //OLD
            //var result = (IEnumerable<EditableLocation>)_requestCache.GetCacheItem("all-locations-by-prop-string", () => allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value));
            //var result = allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value);

            //TODO: Check if a direct search would be faster than LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var result = this.GetLocations().Where(l => l.AllPropertiesDictionary[PropertyAlias] == Value);

            return result;
        }

        public IEnumerable<IndexedLocation> GetLocationsByPropertyValue(string PropertyAlias, int Value)
        {
            //OLD
            //var allLocations = this.GetLocations();
            //var result = (IEnumerable<EditableLocation>)_requestCache.GetCacheItem("all-locations-by-prop-int", () => allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString()));
            //var result = allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            var result = this.GetLocationsByPropertyValue(PropertyAlias, Value.ToString());

            return result;
        }

        public IEnumerable<IndexedLocation> GetLocationsByPropertyValue(string PropertyAlias, DateTime Value)
        {
            //OLD
            //var allLocations = this.GetLocations();
            //var result = (IEnumerable<EditableLocation>)_requestCache.GetCacheItem("all-locations-by-prop-date", () => allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString()));
            //var result = allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            var result = this.GetLocationsByPropertyValue(PropertyAlias, Value.ToString());

            return result;
        }

        public IEnumerable<IndexedLocation> GetLocationsByPostalCode(string PostalCode)
        {
            //OLD
            //Repositories.LocationRepo.GetByPostalCode(postalCode)

            //TODO: Check if a direct search would be faster than using the Repo or LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var result = this.GetLocations().Where(l => l.PostalCode == PostalCode);

            return result;
        }

        public IEnumerable<IndexedLocation> GetLocationsByCountry(string CountryCode, Guid LocTypeKey)
        {
            //TODO: Check if a direct search would be faster than using the Repo or LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var locsByCountry = this.GetLocations().Where(l => l.CountryCode == CountryCode);
            var result = locsByCountry.Where(l => l.LocationTypeKey == LocTypeKey);

            return result;
        }

        public IEnumerable<IndexedLocation> GetLocationsByCountry(string CountryCode)
        {
            //OLD
            //uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(Repositories.LocationRepo.GetByCountry(CountryCode));

            //TODO: Check if a direct search would be faster than using the Repo or LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var result = this.GetLocations().Where(l => l.CountryCode == CountryCode);

            return result;
        }

        public IEnumerable<IndexedLocation> GetAllMissingDbGeo()
        {
            //TODO: Check if a direct search would be faster than using the Repo or LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            //var result = this.GetLocations().Where(l => l.CountryCode == CountryCode);

            var result = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(Repositories.LocationRepo.GetAllMissingDbGeo());
            return result;
        }

        public List<LocationPropertyData> GetAllPropertyData()
        {
           //TODO: Fix this to return IndexPropertyData
           return Repositories.LocationPropertyDataRepo.GetAll().ToList();
        }

        public IEnumerable<IndexedLocation> GetByGeoSearch(double Lat, double Long, int Miles)
        {
            //TODO: see if we can speed this up using Examine Spatial
            var result = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(
                    Repositories.LocationRepo.GetByGeoSearch(Lat, Long, Miles));

            return result;
        }

        public IEnumerable<IndexedLocation> GetNearestLocations(double Lat, double Long, int Qty)
        {
            //TODO: see if we can speed this up using Examine Spatial
            var result = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(Repositories.LocationRepo.GetNearestLocations(Lat, Long, Qty));

            return result;
        }

        public IEnumerable<IndexedLocation> GetNearestLocations(double Lat, double Long, int Qty, Guid LocType)
        {
            //TODO: see if we can speed this up using Examine Spatial
            var result = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(Repositories.LocationRepo.GetNearestLocations(Lat, Long, Qty, LocType));

            return result;
        }

        public IEnumerable<IndexedLocation> GetByGeoSearch(double Lat, double Long, int Miles, Guid LocType)
        {
            //TODO: see if we can speed this up using Examine Spatial
            var result = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(
                    Repositories.LocationRepo.GetByGeoSearch(Lat, Long, Miles, LocType));

            return result;
        }

        #region Paging

        //Check into paging via Page<T> and SQL
        //Via Rusty: 
        //  "Merchello.Web.Search folder" & two methods (Page<Guid> GetPagedKeys()) are from my ProductRepository class in Merchello.Core.Persistence
        //  petapoco's Page<T> object is really nice for paging natively in SQL

        public PagingCollection<IndexedLocation> GetAllPages(int ItemsPerPage, string OrderBy, string SearchTerm, string SortOrder = "ASC")
        {
            //TODO: Check if a direct search would be faster than LINQ
            //var searcher = locationIndexManager.uLocateLocationSearcher;
            //var searchCriteria = searcher.CreateSearchCriteria("*");
            //var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var allLocations = this.GetLocations();
            List<IndexedLocation> workingCollection = new List<IndexedLocation>();

            // WHERE?
            if (SearchTerm != string.Empty)
            {
                workingCollection.AddRange(allLocations.Where(n =>
                   n.Name.Contains(SearchTerm)
                || n.Address1.Contains(SearchTerm)
                || n.Address2.Contains(SearchTerm)
                || n.Locality.Contains(SearchTerm)
                || n.Region.Contains(SearchTerm)
                || n.PostalCode.Contains(SearchTerm)
                || n.CountryCode.Contains(SearchTerm)));
            }
            else
            {
                workingCollection = allLocations.ToList();
            }

            // ORDER?
            // orderedColl = new IOrderedEnumerable<EditableLocation>();
            if (OrderBy != string.Empty)
            {
                switch (OrderBy.ToLower())
                {
                    case "name":
                        workingCollection = SortOrder == "ASC" ? workingCollection.OrderBy(n => n.Name).ToList() : workingCollection.OrderByDescending(n => n.Name).ToList();
                        break;

                    case "locationtype":
                        workingCollection = SortOrder == "ASC" ? workingCollection.OrderBy(n => n.LocationTypeName).ToList() : workingCollection.OrderByDescending(n => n.LocationTypeName).ToList();
                        break;
                }
            }
            else
            {
                workingCollection = SortOrder == "ASC" ? workingCollection.OrderBy(n => n.Name).ToList() : workingCollection.OrderByDescending(n => n.Name).ToList();
            }

            // create paging collection
            PagingCollection<IndexedLocation> pagingColl = new PagingCollection<IndexedLocation>(workingCollection);
            pagingColl.PageSize = ItemsPerPage;

            return pagingColl;
        }

        #endregion

        #endregion

        #region Private - Editable Locations

        private EditableLocation GetEditableLocation(Guid LocationKey)
        {
            var Result = (EditableLocation)_requestCache.GetCacheItem("location-by-key", () => Repositories.LocationRepo.GetByKey(LocationKey));

            //var Result = Repositories.LocationRepo.GetByKey(LocationKey);

            return Result;
        }

        private IEnumerable<EditableLocation> GetAllEditableLocations()
        {
            //var result = (IEnumerable<EditableLocation>)_requestCache.GetCacheItem("all-locations", () => Repositories.LocationRepo.GetAll());
            //var result = Repositories.LocationRepo.GetAll();

            var searcher = locationIndexManager.uLocateLocationSearcher();
            var searchCriteria = searcher.CreateSearchCriteria("*");
            var searchResults = searcher.Search(searchCriteria.SearchIndexType, true);

            var searchedLocations = uLocate.Helpers.Convert.ExamineToSearchedLocations(searchResults);

            var jsonLocations = searchedLocations.Select(n => n.IndexedLocation);
            var result = uLocate.Helpers.Convert.IndexedLocationsToEditableLocations(jsonLocations);
            return result;
        }

        #endregion


    }
}
