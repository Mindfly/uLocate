namespace uLocate.Integration.Tests.Geocoding
{
    using NUnit.Framework;

    using uLocate.Integration.Tests.TestHelpers;

    [TestFixture]
    public class GeographyHelperTests : IntegrationTestBase
    {
        //[Test]
        //public void Can_Convert_A_Coordinate_To_A_Valid_Point_Text()
        //{
        //    //// Arrange
        //    var helper = new GeographyHelper();
        //    var provider = new GoogleMapsGeocodeProvider(new NullCacheProvider());
        //    var response = provider.Geocode(Address);
        //    var factory = new LocatedAddressFactory();
        //    var located = factory.BuildLocatedAddress(Address, response);
            
        //    var geography = SqlGeography.Point(located.Coordinate.Latitude, located.Coordinate.Longitude, Constants.WorldGeodeticSystemSrid);

        //    //// Act
        //    var pointText = helper.GetStPointText(located.Coordinate);

        //    //// Assert
        //    Assert.AreEqual(geography.ToString(), pointText);
        //}

        //[Test]
        //public void Can_Convert_A_Viewport_To_A_Valid_LineString()
        //{
        //    //// Arrange
        //    var helper = new GeographyHelper();
        //    var provider = new GoogleMapsGeocodeProvider(new NullCacheProvider());
        //    var response = provider.Geocode(Address);
        //    var factory = new LocatedAddressFactory();
        //    var located = factory.BuildLocatedAddress(Address, response);

        //    var sqlChars = new SqlChars(helper.GetStLineString(located.Viewport));
        //    var geography = SqlGeography.STLineFromText(sqlChars, Constants.WorldGeodeticSystemSrid);

        //    //// Act
        //    var lineString = helper.GetStLineString(located.Viewport);

        //    //// Assert
        //    Assert.AreEqual(geography.ToString(), lineString);
        //}
    }
}