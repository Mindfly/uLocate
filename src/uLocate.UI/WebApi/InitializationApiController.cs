namespace uLocate.UI.WebApi
{
 
    using System.Web.Http;
    using Umbraco.Web.WebApi;

    //TODO: Add to back-office App area / secure this

    /// <summary>
    /// uLocate initialization api controller.
    /// </summary>
    [Umbraco.Web.Mvc.PluginController("uLocate")]
    public class InitializationApiController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// Initializes the uLocate Database tables
        /// /umbraco/backoffice/uLocate/InitializationApi/InitDb
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
        /// /umbraco/backoffice/uLocate/InitializationApi/DeleteDb
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
        /// /umbraco/backoffice/uLocate/InitializationApi/ResetDb
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
        /// Checks that the uLocate Database tables exist in the current db and Initializes them if missing
        /// /umbraco/backoffice/uLocate/InitializationApi/DbTestInit
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [AcceptVerbs("GET")]
        public bool DbTestInit()
        {
            bool Result = uLocate.Data.Helper.AllTablesInitialized();

            if (!Result)
            {
                Result = uLocate.Data.Helper.InitializeDatabase();
            }

            return Result;
        }
    }
}
