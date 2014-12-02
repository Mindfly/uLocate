namespace uLocate.WebApi
{
    using System;
    using System.Collections.Generic;
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
        ///  /Umbraco/Api/InitializationApi/Test
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public LocationType Test()
        {
            //var Result = Repositories.LocationTypeRepo.GetAll();
            //return Result;

            var NewItem = new LocationType();
            var NewProps = new List<LocationTypeProperty>();
            NewItem.Name = "Business";
            NewItem.Properties = NewProps;

            NewProps.Add(Repositories.CreateLocationTypeProp("BusinessWebsite", "Business Website URL", uLocate.Constants.DataTypeId.TextBox));

            NewItem.Properties = NewProps;

            return NewItem;
        }
    }
}
