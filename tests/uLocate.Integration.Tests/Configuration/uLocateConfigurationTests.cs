using System.Configuration;
using NUnit.Framework;

namespace uLocate.Integration.Tests.Configuration
{
    [TestFixture]
    public class uLocateConfigurationTests
    {
        [Test]
        public void Can_Retrieve_The_uLocate_Section()
        {
            //// Arrange
            
            //// Act
            var section = ConfigurationManager.GetSection("uLocate");

            //// Assert
            Assert.NotNull(section);
        }

    }
}