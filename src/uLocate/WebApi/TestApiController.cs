namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using uLocate.Data;
    using uLocate.Models;
    using uLocate.Persistance;

    using Umbraco.Core;
    using Umbraco.Web.WebApi;

    using Constants = uLocate.Constants;

    /// <summary>
    /// uLocate test api controller.
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class TestApiController : UmbracoAuthorizedApiController
    {

        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/TestPopulateSomeLocationTypes
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public IEnumerable<LocationType> TestPopulateSomeLocationTypes()
        {
            int NewItemId = 0;
            string Msg = "";

            //TEST Add "Business" location type with 2 properties
            var NewItem1 = new LocationType();
            NewItem1.Name = "Business";
            NewItem1.AddProperty("BusinessWebsite", "Business Website URL", uLocate.Constants.DataTypeId.TextBox);
            NewItem1.AddProperty("BusinessHours", "Hours of Operation", uLocate.Constants.DataTypeId.TextBoxMultiple);
            Repositories.LocationTypeRepo.Insert(NewItem1);
            Msg += string.Format("Type '{0}' added. ", NewItem1.Name);

            //TEST Add "Shopping Center" location type with 2 properties
            var NewItem2 = new LocationType();
            NewItem2.Name = "Shopping Center";
            NewItem2.AddProperty("SCName", "Shopping Center Name", uLocate.Constants.DataTypeId.TextBox);
            NewItem2.AddProperty("SCHours", "Hours of Operation", uLocate.Constants.DataTypeId.TextBoxMultiple);
            Repositories.LocationTypeRepo.Insert(NewItem2);
            Msg += string.Format("Type '{0}' added. ", NewItem2.Name);

            //TEST: Return all Location Types
            var Result = Repositories.LocationTypeRepo.GetAll();

            return Result;
        }

        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/TestUpdateLocationType
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public IEnumerable<LocationType> TestUpdateLocationType()
        {
            int NewItemId = 0;
            string Msg = "";

            //TEST: Update a Location Type
            List<LocationType> Result = new List<LocationType>();

            //change the data
            var Lt = Repositories.LocationTypeRepo.GetByName("Shopping Center").FirstOrDefault();
            Lt.Name = "Shopping Mall";
            Repositories.LocationTypeRepo.Update(Lt);

            var Prop = Lt.Properties.Where(p => p.Alias == "SCHours").FirstOrDefault();
            if (Prop != null)
            {
                Prop.Alias = "BusinessHours";
                Repositories.LocationTypePropertyRepo.Update(Prop);
            }

            var Prop2 = Lt.Properties.Where(p => p.Alias == "SCName").FirstOrDefault();
            if (Prop2 != null)
            {
                Prop2.Alias = "SMName";
                Prop2.Name = "Shopping Mall Name";
                Repositories.LocationTypePropertyRepo.Update(Prop2);
            }

            //show new data
            Result.Add(Lt);

            return Result;
        }

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
        [AcceptVerbs("GET")]
        public StatusMessage TestDeleteLocationType(string LookupName = "Business")
        {
            StatusMessage StatusMsg = new StatusMessage();
            StatusMsg.ObjectName = LookupName;

            //TEST Delete a LocationType
            var LookupItem = Repositories.LocationTypeRepo.GetByName(LookupName).FirstOrDefault();
            if (LookupItem != null)
            {
                StatusMsg = Repositories.LocationTypeRepo.Delete(LookupItem);
            }
            else
            {
                StatusMsg.Message = string.Format("'{0}' was not found and can not be deleted.", LookupName);
                StatusMsg.Success = false;
            }

            return StatusMsg;
        }



        /// <summary>
        /// Add two default-type locations, with addresses
        /// /umbraco/backoffice/uLocate/TestApi/TestPopulateSomeLocations
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public IEnumerable<JsonLocation> TestPopulateSomeLocations()
        {
            string Msg = "";

            //TEST Add "Mindfly Office" location 
            var NewItem1Key = uLocate.Helpers.Persistence.CreateLocation("Mindfly Office", true);
            var NewItem1 = Repositories.LocationRepo.GetByKey(NewItem1Key);
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.Address1, "114 W. Magnolia St");
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.Address2, "Suite 300");
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.Locality, "Bellingham");
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.Region, "WA");
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.PostalCode, "98225");
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.CountryCode, "USA");
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.Phone, "360-647-7470");
            NewItem1.AddPropertyData(Constants.DefaultLocPropertyAlias.Email, "hello@mindfly.com");
            uLocate.Helpers.Persistence.UpdateLocation(NewItem1);

            Msg += string.Format("Location '{0}' added. ", NewItem1.Name);

            //TEST Add "Heather's House" location 
            var NewItem2Key = uLocate.Helpers.Persistence.CreateLocation("Heather's House", true);
            var NewItem2 = Repositories.LocationRepo.GetByKey(NewItem2Key);
            NewItem2.AddPropertyData(Constants.DefaultLocPropertyAlias.Address1, "1820 Madison Avenue");
            NewItem2.AddPropertyData(Constants.DefaultLocPropertyAlias.Address2, "8C");
            NewItem2.AddPropertyData(Constants.DefaultLocPropertyAlias.Locality, "New York");
            NewItem2.AddPropertyData(Constants.DefaultLocPropertyAlias.Region, "NY");
            NewItem2.AddPropertyData(Constants.DefaultLocPropertyAlias.PostalCode, "10035");
            NewItem2.AddPropertyData(Constants.DefaultLocPropertyAlias.CountryCode, "USA");
            uLocate.Helpers.Persistence.UpdateLocation(NewItem2);

            Msg += string.Format("Location '{0}' added. ", NewItem2.Name);

            //TEST: Return all Location Types
            var Result = Repositories.LocationRepo.ConvertToJsonLocations(Repositories.LocationRepo.GetAll());

            return Result;
        }


        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/GetPropertyDataRow?PropertyAlias=Phone,LocationKey="000..."
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public LocationPropertyData GetPropertyDataRow(string PropertyAlias, Guid LocationKey)
        {
            var Result = Repositories.LocationPropertyDataRepo.GetByAlias(PropertyAlias, LocationKey);

            return Result;
        }


        /// <summary>
        /// Used for testing
        /// /umbraco/backoffice/uLocate/TestApi/Test
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public Location Test()
        {
            int NewItemId = 0;
            string Msg = "";

            //TEST: Add a new Location 
            var newLoc = new Location("Test Location");
            Repositories.LocationRepo.Insert(newLoc);

            var Result = Repositories.LocationRepo.GetByKey(newLoc.Key);

            return Result;
        }




    }
}
