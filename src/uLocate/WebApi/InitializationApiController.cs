namespace uLocate.WebApi
{
 
    using System.Web.Http;
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
    }
}
