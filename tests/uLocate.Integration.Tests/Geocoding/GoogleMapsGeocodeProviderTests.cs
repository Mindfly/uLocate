using NUnit.Framework;
using uLocate.Caching;
using uLocate.Integration.Tests.TestHelpers;
using uLocate.Models;
using uLocate.Plugins.Geocode.GoogleMaps;

namespace uLocate.Integration.Tests.Geocoding
{
    using System;
    using System.Linq;

    using uLocate.Persistance.Factories;

    [TestFixture]
    public class GoogleMapsGeocodeProviderTests : IntegrationTestBase
    {
        [Test]
        public void Can_GeoCode_An_Address()
        {
            //// Arrage
            var provider = new GoogleMapsGeocodeProvider(new NullCacheProvider());

            //// Act
            var response = provider.Geocode(this.Address);

            //// Assert
            Assert.NotNull(response);
            Console.WriteLine("Lat: {0} - Long: {1}", response.Results.First().Latitude, response.Results.First().Longitude);
        }

        //[Test]
        //public void Can_Build_A_LocatedAddress()
        //{
        //    //// Arrange
        //    var provider = new GoogleMapsGeocodeProvider(new NullCacheProvider());
        //    var response = provider.Geocode(this.Address);

        //    //// Act
        //    var factory = new LocatedAddressFactory();
        //    var locatedAddress = factory.BuildLocatedAddress(Address, response);

        //    //// Assert
        //    Assert.NotNull(locatedAddress);

        //    // AutoMapper Mappings
        //    Assert.AreEqual(Address.Address1, locatedAddress.Address1);
        //    Assert.AreEqual(Address.Address2, locatedAddress.Address2);
        //    Assert.AreEqual(Address.Locality, locatedAddress.Locality);
        //    Assert.AreEqual(Address.Region, locatedAddress.Region);
        //    Assert.AreEqual(Address.PostalCode, locatedAddress.PostalCode);
        //    Assert.AreEqual(Address.CountryCode, locatedAddress.CountryCode);

        //    // Coordinates
        //    Assert.NotNull(locatedAddress.Coordinate);
        //    Assert.NotNull(locatedAddress.Viewport);
        //}
    }
}