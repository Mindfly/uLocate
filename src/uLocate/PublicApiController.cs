namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    using uLocate.Indexer;
    using uLocate.Models;
    using uLocate.Persistance;
    using uLocate.Services;

    using Umbraco.Core.Logging;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// uLocate test api controller.
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class PublicApiController : UmbracoApiController
    {
        private LocationService locationService = new LocationService();
        private LocationTypeService locationTypeService = new LocationTypeService();

        /// /umbraco/backoffice/uLocate/PublicApi/Test
        [System.Web.Http.AcceptVerbs("GET")]
        public bool Test()
        {
            LogHelper.Info<PublicApiController>("Test STARTED/ENDED");
            return true;
        }

        #region Indexing

        /// /umbraco/uLocate/PublicApi/ReIndexAll

        [AcceptVerbs("GET")]
        public StatusMessage ReIndexAll()
        {
            LogHelper.Info<PublicApiController>("ReIndex STARTED");
            var result = locationService.ReindexAllLocations();
            LogHelper.Info<PublicApiController>("ReIndex ENDED");

            return result;
        }

        #endregion

        #region Locations

        ///// <summary>
        ///// Add two default-type locations, with addresses
        ///// /umbraco/backoffice/uLocate/PublicApi/TestCreateALocation?LocationName=xxx
        ///// </summary>
        ///// <returns>
        ///// The <see cref="List{T}"/>.
        ///// </returns>
        //[AcceptVerbs("GET")]
        //public IEnumerable<IndexedLocation> TestCreateALocation(string LocationName)
        //{
        //    LogHelper.Info<PublicApiController>(string.Format("TestCreateALocation STARTED: {0}", LocationName));
        //    string Msg = "";

        //    //TEST Add location 
        //    var newItem = locationService.CreateLocation(LocationName);

        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.Address1, "114 W. Magnolia St");
        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.Address2, "Suite 300");
        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.Locality, "Bellingham");
        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.Region, "WA");
        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.PostalCode, "98225");
        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.CountryCode, "USA");
        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.Phone, "360-647-7470");
        //    newItem.AddPropertyData(Constants.DefaultLocPropertyAlias.Email, "hello@mindfly.com");

        //    locationService.UpdateLocation(newItem);

        //    Msg += string.Format("Location '{0}' added. ", newItem.Name);

        //    LogHelper.Info<PublicApiController>(Msg);

        //    //TEST: Return all Locations with that name
        //    var result = locationService.GetLocations(LocationName);
        //    //var Result = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(Repositories.LocationRepo.GetAll());

        //    LogHelper.Info<PublicApiController>(string.Format("TestCreateALocation COMPLETE: {0} Key={1}", LocationName, newItem.Key));
        //    return result;
        //}


        ///// /umbraco/backoffice/uLocate/TestApi/Search?Keyword=xxx

        //[AcceptVerbs("GET")]
        //public Examine.ISearchResults Search(string Keyword)
        //{
        //    var searchParams = new uLocate.Search.SearchParameters();
        //    searchParams.SearchTerm = Keyword;

        //    var locationSearch = new uLocate.Search.ExamineSearch(searchParams);
        //    var results = locationSearch.ResultsMultiRelevance();
        //    //.Search(queryType, searchTerms, titleProperties, bodyProperties, rootNodes, titleLinkProperties, summaryProperties, contextHighlighting, summaryLength, pageNumber, pageLength, fuzzyness, useWildcards);
        //    //IEnumerable<IndexedLocation> 

        //    return results;
        //}

        ///// /umbraco/backoffice/uLocate/TestApi/SearchByService?Keyword1=xxx&Keyword2=

        //[AcceptVerbs("GET")]
        //public IEnumerable<IndexedLocation> SearchByService(string Keyword1, string Keyword2)
        //{
        //    bool Option = true;
        //    if (Keyword2 == "false")
        //    {
        //        Option = false;
        //    }
        //    var results = locationService.GetLocationsByPostalCode(Keyword1, Option);
        //    //.Search(queryType, searchTerms, titleProperties, bodyProperties, rootNodes, titleLinkProperties, summaryProperties, contextHighlighting, summaryLength, pageNumber, pageLength, fuzzyness, useWildcards);
        //    //IEnumerable<IndexedLocation> 

        //    return results;
        //}

        #endregion

        #region LocationTypes
        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/TestPopulateSomeLocationTypes
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //[AcceptVerbs("GET")]
        //public IEnumerable<LocationType> TestPopulateSomeLocationTypes()
        //{
        //    int NewItemId = 0;
        //    string Msg = "";

        //    //TEST Add "Business" location type with 2 properties
        //    var NewItem1 = new LocationType();
        //    NewItem1.Name = "Business";
        //    NewItem1.AddProperty("BusinessWebsite", "Business Website URL", uLocate.Constants.DataTypeId.TextBox);
        //    NewItem1.AddProperty("BusinessHours", "Hours of Operation", uLocate.Constants.DataTypeId.TextBoxMultiple);
        //    locationTypeService.Create();
        //    Repositories.LocationTypeRepo.Insert(NewItem1);
        //    Msg += string.Format("Type '{0}' added. ", NewItem1.Name);

        //    //TEST Add "Shopping Center" location type with 2 properties
        //    var NewItem2 = new LocationType();
        //    NewItem2.Name = "Shopping Center";
        //    NewItem2.AddProperty("SCName", "Shopping Center Name", uLocate.Constants.DataTypeId.TextBox);
        //    NewItem2.AddProperty("SCHours", "Hours of Operation", uLocate.Constants.DataTypeId.TextBoxMultiple);
        //    Repositories.LocationTypeRepo.Insert(NewItem2);
        //    Msg += string.Format("Type '{0}' added. ", NewItem2.Name);

        //    //TEST: Return all Location Types
        //    var Result = Repositories.LocationTypeRepo.GetAll();

        //    return Result;
        //}

        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/TestUpdateLocationType
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //[AcceptVerbs("GET")]
        //public IEnumerable<LocationType> TestUpdateLocationType()
        //{
        //    int NewItemId = 0;
        //    string Msg = "";

        //    //TEST: Update a Location Type
        //    List<LocationType> Result = new List<LocationType>();

        //    //change the data
        //    var Lt = Repositories.LocationTypeRepo.GetByName("Shopping Center").FirstOrDefault();
        //    Lt.Name = "Shopping Mall";
        //    Repositories.LocationTypeRepo.Update(Lt);

        //    var Prop = Lt.Properties.Where(p => p.Alias == "SCHours").FirstOrDefault();
        //    if (Prop != null)
        //    {
        //        Prop.Alias = "BusinessHours";
        //        Repositories.LocationTypePropertyRepo.Update(Prop);
        //    }

        //    var Prop2 = Lt.Properties.Where(p => p.Alias == "SCName").FirstOrDefault();
        //    if (Prop2 != null)
        //    {
        //        Prop2.Alias = "SMName";
        //        Prop2.Name = "Shopping Mall Name";
        //        Repositories.LocationTypePropertyRepo.Update(Prop2);
        //    }

        //    //show new data
        //    Result.Add(Lt);

        //    return Result;
        //}

        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/TestDeleteLocationType?LookupName=name
        /// </summary>
        /// <param name="LookupName">
        /// The Lookup Name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //[AcceptVerbs("GET")]
        //public StatusMessage TestDeleteLocationType(string LookupName = "Business")
        //{
        //    StatusMessage StatusMsg = new StatusMessage();
        //    StatusMsg.ObjectName = LookupName;

        //    //TEST Delete a LocationType
        //    var LookupItem = Repositories.LocationTypeRepo.GetByName(LookupName).FirstOrDefault();
        //    if (LookupItem != null)
        //    {
        //        StatusMsg = Repositories.LocationTypeRepo.Delete(LookupItem);
        //    }
        //    else
        //    {
        //        StatusMsg.Message = string.Format("'{0}' was not found and can not be deleted.", LookupName);
        //        StatusMsg.Success = false;
        //    }

        //    return StatusMsg;
        //}


        #endregion

        #region Other

        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/GetPropertyDataRow?PropertyAlias=Phone,LocationKey="000..."
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //[AcceptVerbs("GET")]
        //public LocationPropertyData GetPropertyDataRow(string PropertyAlias, Guid LocationKey)
        //{
        //    var Result = Repositories.LocationPropertyDataRepo.GetByAlias(PropertyAlias, LocationKey);

        //    return Result;
        //}


        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/Test
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //[AcceptVerbs("GET")]
        //public EditableLocation Test()
        //{
        //    int NewItemId = 0;
        //    string Msg = "";

        //    //TEST: Add a new Location 
        //    var newLoc = new EditableLocation("Test Location");
        //    Repositories.LocationRepo.Insert(newLoc);

        //    var Result = Repositories.LocationRepo.GetByKey(newLoc.Key);

        //    return Result;
        //}

        #endregion
    }
}
