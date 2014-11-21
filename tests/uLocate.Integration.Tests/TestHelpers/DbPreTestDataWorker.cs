using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;

namespace uLocate.Integration.Tests.TestHelpers
{
    public class DbPreTestDataWorker
    {
        public IDatabaseUnitOfWorkProvider UnitOfWorkProvider { get; private set; }

        public DbPreTestDataWorker()
        {
            SqlSyntaxProviderTestHelper.EstablishSqlSyntax();

            UnitOfWorkProvider = new PetaPocoUnitOfWorkProvider();
        }
    }
}