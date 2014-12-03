namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using uLocate.Models;
    using uLocate.Persistance;
    using uLocate.Data;
    using Umbraco.Core;
    using Umbraco.Web.WebApi;

    //TODO: Add to back-office App area / secure this

    /// <summary>
    /// uLocate initialization api controller.
    /// </summary>
    public class InitializationApiController : UmbracoApiController
    {
        /// <summary>
        /// Initializes the uLocate Database tables
        /// /Umbraco/Api/InitializationApi/InitDb
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public bool InitDb()
        {
            bool Result = uLocate.Data.Helper.InitializeDatabase();

            return Result;
        }

        /// <summary>
        /// Deletes the uLocate Database tables
        /// /Umbraco/Api/InitializationApi/DeleteDb
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public bool DeleteDb()
        {
            bool Result = uLocate.Data.Helper.DeleteDatabase();

            return Result;
        }

        /// <summary>
        /// Used for testing (deletes &amp; re-creates database)
        ///  /Umbraco/Api/InitializationApi/ResetDb
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public bool ResetDb()
        {
            bool Result = uLocate.Data.Helper.DeleteDatabase();
            if (Result)
            {
                Result = uLocate.Data.Helper.InitializeDatabase();
            }

            return Result;
        }

        /// <summary>
        /// Used for testing
        ///  /Umbraco/Api/InitializationApi/TestPopulateSomeLocationTypes
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
            Repositories.LocationTypeRepo.Insert(NewItem2, out NewItemId);
            Msg += string.Format("Type '{0}' added. ", NewItem2.Name);

            //TEST: Return all Location Types
            var Result = Repositories.LocationTypeRepo.GetAll();

            return Result;
        }

        /// <summary>
        /// Used for testing
        ///  /Umbraco/Api/InitializationApi/TestUpdateLocationType
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
            var Lt = Repositories.LocationTypeRepo.GetById(3);
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
            Result.Add(Repositories.LocationTypeRepo.GetById(3));

            return Result;
        }

        /// <summary>
        /// Used for testing
        ///  /Umbraco/Api/InitializationApi/Test
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public IEnumerable<LocationType> Test()
        {
            int NewItemId = 0;
            string Msg = "";

            //TEST Delete a LocationType by Id
            //var LookupItem = Repositories.LocationTypeRepo.GetById(14);
            //string Msg = string.Format("'{0}' has been deleted.", LookupItem.Name);
            //Repositories.LocationTypeRepo.Delete(14);

            //TEST: Return all Location Types
            var Result = Repositories.LocationTypeRepo.GetAll();
            //return Result;


            return Result;
        }
    }
}
