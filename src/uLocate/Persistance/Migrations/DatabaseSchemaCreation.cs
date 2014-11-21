namespace uLocate.Persistance.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Rdbms;
    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// The database schema creation.
    /// </summary>
    internal class DatabaseSchemaCreation
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSchemaCreation"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        public DatabaseSchemaCreation(Database database)
        {
            _database = database;
        }

        /// <summary>
        /// Collection of tables to be added to the database
        /// </summary>
        private static readonly Dictionary<int, Type> OrderedTables = new Dictionary<int, Type>
        {
            { 0, typeof(LocationTypeDefinitionDto) },
            { 1, typeof(LocationDto) }
        };


        /// <summary>
        /// Installs the Merchello tables
        /// </summary>
        public void InitializeDatabaseSchema()
        {
            foreach (var item in OrderedTables.OrderBy(x => x.Key))
            {
                _database.CreateTable(false, item.Value);
            }
        }

        /// <summary>
        /// Uninstalls the database tables.  Used in package uninstall
        /// </summary>
        public void UninstallDatabaseSchema()
        {
            foreach (var item in OrderedTables.OrderByDescending(x => x.Key))
            {
                var tableNameAttribute = item.Value.FirstAttribute<TableNameAttribute>();

                string tableName = tableNameAttribute == null ? item.Value.Name : tableNameAttribute.Value;

                try
                {
                    if (_database.TableExist(tableName))
                    {
                        _database.DropTable(tableName);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}