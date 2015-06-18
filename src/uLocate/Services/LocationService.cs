namespace uLocate.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    using ClientDependency.Core;

    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Core.Cache;

    /// <summary>
    /// Functions related to looking up and editing data which can be called from Api Controllers etc.
    /// </summary>
    public class LocationService
    {
        private ICacheProvider _requestCache = ApplicationContext.Current.ApplicationCache.RequestCache;

        private LocationTypeService locTypeService = new LocationTypeService();

        //public LocationService()
        //{
        //    _requestCache = ApplicationContext.Current.ApplicationCache.RequestCache;
        //}

        #region Location-Related - CUD

        public StatusMessage DeleteLocation(Guid LocationKey)
        {
            var Result = Repositories.LocationRepo.Delete(LocationKey);

            return Result;
        }

        public Guid CreateLocation(string LocationName, bool UpdateIfFound = false)
        {
            var Result = CreateLocation(LocationName, uLocate.Constants.DefaultLocationTypeKey, UpdateIfFound);

            return Result;
        }

        public Guid CreateLocation(string LocationName, Guid LocationTypeGuid, bool UpdateIfFound = false)
        {
            bool DoUpdate = false;
            Guid LocType;

            if (LocationTypeGuid != Guid.Empty)
            {
                LocType = LocationTypeGuid;
            }
            else
            {
                LocType = uLocate.Constants.DefaultLocationTypeKey;
            }

            if (UpdateIfFound)
            {
                //Lookup first
                var matchingLocations = Repositories.LocationRepo.GetByName(LocationName);
                if (matchingLocations.Any())
                {
                    Location lookupLoc = matchingLocations.FirstOrDefault();
                    lookupLoc.Name = LocationName;
                    lookupLoc.LocationTypeKey = LocType;
                    Repositories.LocationRepo.Update(lookupLoc);
                    return lookupLoc.Key;
                }
            }

            Location newLoc = new Location(LocationName, LocType);
            Repositories.LocationRepo.Insert(newLoc);
            return newLoc.Key;
        }

        public Location UpdateLocation(Location UpdatedLocation)
        {
            Repositories.LocationRepo.Update(UpdatedLocation);

            var Result = Repositories.LocationRepo.GetByKey(UpdatedLocation.Key);

            return Result;
        }

        public StatusMessage UpdateGeographyData(Location LocationToUpdate)
        {
            var returnMsg = new StatusMessage();

            try
            {
                Repositories.LocationRepo.UpdateLatLong(LocationToUpdate);
            }
            catch (Exception exLatLong)
            {
                returnMsg.Success = false;
                returnMsg.Code = "ErrorGeoCode";
                returnMsg.Message = string.Format("{0} location was unable to be updated. Error while geo-coding.", LocationToUpdate.Name);
                returnMsg.RelatedException = exLatLong;
            }

            try
            {
                Repositories.LocationRepo.UpdateDbGeography(LocationToUpdate);
            }
            catch (Exception eDb)
            {
                returnMsg.Success = false;
                returnMsg.Code = "ErrorDbGeography";
                returnMsg.Message = string.Format("{0} location was unable to be updated. Error while updating database geography.", LocationToUpdate.Name);
                returnMsg.RelatedException = eDb;
            }

            returnMsg.Success = true;
            returnMsg.Message = string.Format("{0} location was updated.", LocationToUpdate.Name);

            return returnMsg;
        }

        #endregion

        #region Location Querying

        public Location GetLocation(Guid LocationKey)
        {
            var Result = (Location)_requestCache.GetCacheItem("location-by-key", () => Repositories.LocationRepo.GetByKey(LocationKey));

            //var Result = Repositories.LocationRepo.GetByKey(LocationKey);

            return Result;
        }

        public StatusMessage CountLocations<TKey>(Guid LocationTypeKey, Func<Location, TKey> GroupingProperty)
        {
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

        public IEnumerable<Location> GetAllLocations()
        {
            var result = (IEnumerable<Location>)_requestCache.GetCacheItem("all-locations", () => Repositories.LocationRepo.GetAll());
            //var result = Repositories.LocationRepo.GetAll();

            return result;
        }

        public IEnumerable<Location> GetLocations(Guid LocationTypeKey)
        {
            var result = (IEnumerable<Location>)_requestCache.GetCacheItem("all-locations-by-type", () => Repositories.LocationRepo.GetByType(LocationTypeKey));
            //var result = Repositories.LocationRepo.GetByType(LocationTypeKey);

            return result;
        }

        public IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, string Value)
        {
            var allLocations = this.GetAllLocations();

            var result = (IEnumerable<Location>)_requestCache.GetCacheItem("all-locations-by-prop-string", () => allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value));

            //var result = allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value);

            return result;
        }

        public IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, int Value)
        {
            var allLocations = this.GetAllLocations();

            var result = (IEnumerable<Location>)_requestCache.GetCacheItem("all-locations-by-prop-int", () => allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString()));

            //var result = allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            return result;
        }

        public IEnumerable<Location> GetLocationsByPropertyValue(string PropertyAlias, DateTime Value)
        {
            var allLocations = this.GetAllLocations();

            var result = (IEnumerable<Location>)_requestCache.GetCacheItem("all-locations-by-prop-date", () => allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString()));

            //var result = allLocations.Where(l => l.CustomProperties[PropertyAlias] == Value.ToString());

            return result;
        }

        public PagingCollection<JsonLocation> GetAllPages(int ItemsPerPage, string OrderBy, string SearchTerm)
        {
            var allLocations = this.GetAllLocations();
            List<Location> workingCollection = new List<Location>();
            
            //WHERE?
            if (SearchTerm != string.Empty)
            {
                workingCollection.AddRange(allLocations.Where(n => 
                    n.Name.Contains(SearchTerm)
                || n.Address.Address1.Contains(SearchTerm)
                || n.Address.Address2.Contains(SearchTerm) 
                || n.Address.Locality.Contains(SearchTerm)
                || n.Address.Region.Contains(SearchTerm) 
                || n.Address.PostalCode.Contains(SearchTerm)
                || n.Address.CountryCode.Contains(SearchTerm)));

                //workingCollection.AddRange(allLocations.Where(n => n.Name.Contains(SearchTerm)));
                //workingCollection.AddRange(allLocations.Where(n => n.Address.Address1.Contains(SearchTerm)));
                //workingCollection.AddRange(allLocations.Where(n => n.Address.Address2.Contains(SearchTerm)));
                //workingCollection.AddRange(allLocations.Where(n => n.Address.Locality.Contains(SearchTerm)));
                //workingCollection.AddRange(allLocations.Where(n => n.Address.Region.Contains(SearchTerm)));
                //workingCollection.AddRange(allLocations.Where(n => n.Address.PostalCode.Contains(SearchTerm)));
                //workingCollection.AddRange(allLocations.Where(n => n.Address.CountryCode.Contains(SearchTerm)));
            }
            else
            {
                workingCollection = allLocations.ToList();
            }

            //ORDER?
           // orderedColl = new IOrderedEnumerable<Location>();
            if (OrderBy != string.Empty)
            {
                switch (OrderBy.ToLower())
                {
                    case "name":
                        workingCollection = workingCollection.OrderBy(n => n.Name).ToList();
                        break;

                    case "locationtype":
                         workingCollection = workingCollection.OrderBy(n => n.LocationType.Name).ToList();
                        break;
                }
            }
            else
            {
                workingCollection = workingCollection.OrderBy(n => n.Name).ToList();
            }
            
            // create paging collection
            PagingCollection<JsonLocation> pagingColl = new PagingCollection<JsonLocation>(Repositories.LocationRepo.ConvertToJsonLocations(workingCollection));
            pagingColl.PageSize = ItemsPerPage;

            return pagingColl;
        } 

        #endregion
    }
}
