namespace uLocate.Data
{
    using System;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// Class to manage default data insertion
    /// </summary>
    internal class DatabaseDefaultDataInsert
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDefaultDataInsert"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        public DatabaseDefaultDataInsert(Database database)
        {
            _database = database;
        }

        /// <summary>
        /// Runs all methods to insert default data
        /// </summary>
        public void InitializeDefaultData()
        {
            LogHelper.Info<DatabaseDefaultDataInsert>(string.Format("Initializing default data..."));

            CreateAllowedDataTypesData();
            CreateLocationTypeData();
            CreateLocationTypePropertyData();
        }

        /// <summary>
        /// Inserts data for table "uLocate_AllowedDataTypes"
        /// </summary>
        private void CreateAllowedDataTypesData()
        {
            LogHelper.Info<DatabaseDefaultDataInsert>(string.Format("CreateAllowedDataTypesData() running..."));

            string TableName = "uLocate_AllowedDataTypes";
            string PrimaryKeyFieldName = "Id";

            LogHelper.Info<DatabaseDefaultDataInsert>(string.Format("Adding data for table '{0}'...", TableName));

            //uLocate.Constants includes the list of allowed datatypes
            foreach (var DataType in uLocate.Constants.AllowedDataTypesDictionary)
            {
                _database.Insert(TableName, PrimaryKeyFieldName, new AllowedDataTypesDto() { UmbracoDataTypeId = DataType.Value });
            }
        }

        /// <summary>
        /// Inserts data for table "uLocate_LocationType"
        /// </summary>
        private void CreateLocationTypeData()
        {
            LogHelper.Info<DatabaseDefaultDataInsert>(string.Format("CreateLocationTypeData() running..."));

            string TableName = "uLocate_LocationType";
            string PrimaryKeyFieldName = "Id";

            LogHelper.Info<DatabaseDefaultDataInsert>(string.Format("Adding data for table '{0}'...", TableName));

            _database.Insert(TableName, PrimaryKeyFieldName, new LocationTypeDto() { Name = "Default" });
        }

        /// <summary>
        /// Inserts data for table "uLocate_LocationTypeProperty"
        /// </summary>
        private void CreateLocationTypePropertyData()
        {
            LogHelper.Info<DatabaseDefaultDataInsert>(string.Format("CreateLocationTypePropertyData() running..."));

            string TableName = "uLocate_LocationTypeProperty";
            string PrimaryKeyFieldName = "Id";

            LogHelper.Info<DatabaseDefaultDataInsert>(string.Format("Adding data for table '{0}'...", TableName));

            //'Default' Properties
            foreach (var Prop in uLocate.Constants.DefaultLocationTypeProperties)
            {
                _database.Insert(
                    TableName,
                    PrimaryKeyFieldName,
                    new LocationTypePropertyDto
                        {
                            LocationTypeId = Prop.LocationTypeId,
                            Alias = Prop.Alias,
                            Name = Prop.Name,
                            UmbracoDataTypeId = Prop.UmbracoDataTypeId,
                            SortOrder = Prop.SortOrder
                        });
            }
        }

    }
}
