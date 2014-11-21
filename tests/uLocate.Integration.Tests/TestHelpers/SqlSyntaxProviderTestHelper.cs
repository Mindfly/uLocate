using System;
using Umbraco.Core.Persistence.SqlSyntax;

namespace uLocate.Integration.Tests.TestHelpers
{
    internal class SqlSyntaxProviderTestHelper
    {
        public static void EstablishSqlSyntax()
        {
            try
            {
                var syntaxtest = SqlSyntaxContext.SqlSyntaxProvider;
            }
            catch (Exception)
            {
                SqlSyntaxContext.SqlSyntaxProvider = new SqlServerSyntaxProvider();
            }
        }
    }
}