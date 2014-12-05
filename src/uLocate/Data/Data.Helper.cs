namespace uLocate.Data
{
    using System;
    using System.Data.SqlClient;

    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Static uLocate Data Helpers
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Gets the current umbraco database (shorthand for 'Umbraco.Core.ApplicationContext.Current.DatabaseContext.Database')
        /// </summary>
        //public static UmbracoDatabase ThisDatabase 
        //{
        //    get
        //    {
        //        return Umbraco.Core.ApplicationContext.Current.DatabaseContext.Database;
        //    }
        //}

        /// <summary>
        /// Gets the current umbraco cache (shorthand forUmbraco.Core.ApplicationContext.Current.ApplicationCache.RuntimeCache)
        /// </summary>
        public static IRuntimeCacheProvider ThisCache 
        {
            get
            {
                return Umbraco.Core.ApplicationContext.Current.ApplicationCache.RuntimeCache;
            }
        }

        /// <summary>
        /// Static method to create the uLocate database tables and insert default data
        /// </summary>
        /// <returns>
        /// <see cref="bool"/> indicating success (no exceptions)
        /// </returns>
        public static bool InitializeDatabase()
        {
            // 1. Schema Creation
            try
            {
                var DbSchema = new DatabaseSchemaCreation(Umbraco.Core.ApplicationContext.Current.DatabaseContext.Database);
                DbSchema.InitializeDatabaseSchema();
            }
            //catch (SqlException SqlEx)
            //{
            //    if (SqlEx.Message.Contains(
            //            "There is already an object named 'FK_uLocateLocationTypeProperty_cmsDataType' in the database."))
            //    {
            //        var message = string.Concat("uLocate.Data.Helper.InitializeDatabase - Schema Creation: ", SqlEx);
            //        LogHelper.Info(typeof(uLocate.Data.Helper), message);
            //    }
            //    else
            //    {
            //        var message = string.Concat("uLocate.Data.Helper.InitializeDatabase - Schema Creation Error: ", SqlEx);
            //        LogHelper.Error(typeof(uLocate.Data.Helper), message, SqlEx);

            //        return false;
            //    }
            //}
            catch (Exception ex)
            {
                var message = string.Concat("uLocate.Data.Helper.InitializeDatabase - Schema Creation Error: ", ex);
                LogHelper.Error(typeof(uLocate.Data.Helper), message, ex);

                return false;
            }

            // 2. Default Data
            try
            {
                var DbData =
                    new DatabaseDefaultDataInsert(Umbraco.Core.ApplicationContext.Current.DatabaseContext.Database);
                DbData.InitializeDefaultData();
            }
            catch (Exception ex)
            {
                var message = string.Concat("uLocate.Data.Helper.InitializeDatabase - Default Data Error: ", ex);
                LogHelper.Error(typeof(uLocate.Data.Helper), message, ex);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Static method to delete the uLocate Database Tables
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool DeleteDatabase()
        {
            bool Result = true;
            try
            {
                var DbSchema = new DatabaseSchemaCreation(Umbraco.Core.ApplicationContext.Current.DatabaseContext.Database);
                Result = DbSchema.UninstallDatabaseSchema();
            }
            catch (Exception ex)
            {
                var message = string.Concat("uLocate.Data.Helper.DeleteDatabase() Error: ", ex);
                LogHelper.Error(typeof(uLocate.Data.Helper), message, ex);
                Result= false;
            }

            return Result;
        }
    }
}
