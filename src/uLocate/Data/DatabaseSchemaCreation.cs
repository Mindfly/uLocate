namespace uLocate.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using uLocate.Models;

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
        private Database _database;

        /// <summary>
        /// Collection of tables to be added to the database
        /// </summary>
        private static readonly Dictionary<int, Type> OrderedTables = new Dictionary<int, Type>
        {
            { 0, typeof(LocationTypeDto) },
            { 1, typeof(LocationDto) },
            { 2, typeof(LocationTypePropertyDto) },
            { 3, typeof(LocationPropertyDataDto) }

            //{ 4, typeof(AllowedDataTypesDto) }
        };

        private void SpecialSchemaUpdating()
        {
            //For custom table fields which can't be covered by the Dto
            
            //EditableLocation Table
            string TableName = Data.Helper.GetDtoTableName(typeof(LocationDto));

            if (_database.TableExist(TableName))
            {
                var sql = string.Format("ALTER TABLE {0} ADD GeogCoordinate geography NULL ;", TableName);
                _database.Execute(sql);

                var sql2 = string.Format("CREATE SPATIAL INDEX SIndx_SpatialTable_geography_col1 ON {0} ([GeogCoordinate]);", TableName);
                _database.Execute(sql2);
            }
        }



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
        /// Tests that all uLocate DB tables exist
        /// </summary>
        /// <returns>
        /// A <see cref="bool"/> indicating whether all tables exist or not
        /// </returns>
        public bool TablesInitialized()
        {
            foreach (var item in OrderedTables.OrderBy(x => x.Key))
            {
                var TableType = item.Value;              
                var TableAttrib = (TableNameAttribute) Attribute.GetCustomAttribute(TableType, typeof(TableNameAttribute));
                string TableName = TableAttrib.Value;

                if (!_database.TableExist(TableName))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Creates the database tables
        /// </summary>
        public void InitializeDatabaseSchema()
        {
            foreach (var item in OrderedTables.OrderBy(x => x.Key))
            {
                var TableType = item.Value;              
                var TableAttrib = (TableNameAttribute) Attribute.GetCustomAttribute(TableType, typeof(TableNameAttribute));
                string TableName = TableAttrib.Value;

                var message = string.Concat("About to create Table '", TableName, "'");
                LogHelper.Info(typeof(DatabaseSchemaCreation), message);

                if (!_database.TableExist(TableName))
                {
                    try
                    {
                        //Create DB table - and set overwrite to false
                        _database.CreateTable(false, TableType);
                    }
                    catch (Exception ex)
                    {
                        message = string.Concat("Unable to create table '", TableName, "': ", ex);
                        LogHelper.Error(typeof(DatabaseSchemaCreation), message, ex);

                        throw;
                    }
                }


                message = string.Concat("Successfully created Table '", TableName, "'");
                LogHelper.Info(typeof(DatabaseSchemaCreation), message);
            }

            SpecialSchemaUpdating();
        }



        /// <summary>
        /// Deletes the database tables.  (Used in package un-install)
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool UninstallDatabaseSchema()
        {
            bool Result = true;

            // Delete Tables
            foreach (var item in OrderedTables.OrderByDescending(x => x.Key))
            {
                var tableNameAttribute = item.Value.FirstAttribute<TableNameAttribute>();

                string TableName = tableNameAttribute == null ? item.Value.Name : tableNameAttribute.Value;

                try
                {
                    if (_database.TableExist(TableName))
                    {
                        _database.DropTable(TableName);
                        var message = string.Concat("uLocate.Data.DatabaseSchemaCreation.UninstallDatabaseSchema - Deleted Table '", TableName, "'");
                        LogHelper.Info(typeof(DatabaseSchemaCreation), message);
                    }
                }
                catch (Exception ex)
                {
                    var message = string.Concat("uLocate.Data.DatabaseSchemaCreation.UninstallDatabaseSchema - Delete Tables Error: ", ex);
                    LogHelper.Error(typeof(DatabaseSchemaCreation), message, ex);
                    Result = false;
                }
            }

            // Delete Extra Constraints
            //foreach (var item in ConnectedConstraints)
            //{
            //    string ConstraintName = item.Key;
            //    string TableName = item.Value;
            //    string sqlTest = string.Format("SELECT count(object_id) AS Match FROM sys.objects WHERE type_desc LIKE '%CONSTRAINT' AND OBJECT_NAME(parent_object_id)='{0}' AND OBJECT_NAME(object_id)='{1}' ", TableName, ConstraintName);
            //    string sqlDrop = string.Format("ALTER TABLE {0} DROP CONSTRAINT {1};", TableName, ConstraintName);

            //    try
            //    {
            //        var Matching = _database.ExecuteScalar<int>(sqlTest);
            //        if (Matching > 0)
            //        {
            //            _database.Execute(sqlDrop);
            //            var message = string.Concat("uLocate.Data.DatabaseSchemaCreation.UninstallDatabaseSchema - Deleted Constraint '", ConstraintName, "'");
            //            LogHelper.Info(typeof(DatabaseSchemaCreation), message);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        var message = string.Concat("uLocate.Data.DatabaseSchemaCreation.UninstallDatabaseSchema - Delete Extra Constraints Error: ", ex);
            //        LogHelper.Error(typeof(DatabaseSchemaCreation), message, ex);
            //        Result = false;
            //    }
            //}

            return Result;
        }
    }
}