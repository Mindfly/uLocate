using NUnit.Framework;
using uLocate.Integration.Tests.TestHelpers;
using uLocate.Persistance.Migrations;
using Umbraco.Core.Persistence;

namespace uLocate.Integration.Tests.Database
{
    using umbraco.cms.businesslogic.packager;

    [TestFixture]
    public class DatabaseSchemaCreationTests : IntegrationTestBase
    {

        [Test]
        public void Successfully_Create_Default_Database_Schema()
        {
            //// Arrange
            var creation = new DatabaseSchemaCreation(Database);

            //// Act
            creation.InitializeDatabaseSchema();           
        }

        [Test]
        public void Successfully_Uninstall_The_Database()
        {
            //// Arrange
            var creation = new DatabaseSchemaCreation(Database);

            //// Act
            creation.UninstallDatabaseSchema();
        }
    }
}