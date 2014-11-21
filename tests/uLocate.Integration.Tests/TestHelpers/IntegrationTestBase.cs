namespace uLocate.Integration.Tests.TestHelpers
{
    using NUnit.Framework;

    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Persistence;

    public abstract class IntegrationTestBase
    {
        public UmbracoDatabase Database
        {
            get
            {
                return DbPreTestDataWorker.UnitOfWorkProvider.GetUnitOfWork().Database;
            } 
        }

        public DbPreTestDataWorker DbPreTestDataWorker { get; private set; }

        protected IntegrationTestBase()
        {
            DbPreTestDataWorker = new DbPreTestDataWorker();

            AutomapperMappings.CreateMappings();
        }


        protected IAddress Address;

        [TestFixtureSetUp]
        public virtual void Init()
        {
            this.Address = new Address()
            {
                Address1 = "114 W. Magnolia St.",
                Address2 = "Suite 300",
                Locality = "Bellingham",
                Region = "WA",
                PostalCode = "98225",
                CountryCode = "US"
            };
        }

    }
}