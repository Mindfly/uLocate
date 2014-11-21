namespace uLocate.Persistance.Factories
{
    using System;
    using System.Linq;

    using uLocate.Helpers;
    using uLocate.Models;
    using uLocate.Models.Rdbms;

    /// <summary>
    /// The location factory.
    /// </summary>
    internal class LocationFactory
    {
        /// <summary>
        /// The _geography.
        /// </summary>
        private readonly GeographyHelper _geography;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationFactory"/> class.
        /// </summary>
        public LocationFactory()
        {
            _geography = new GeographyHelper();
        }

        /// <summary>
        /// Builds a location from an <see cref="IAddress"/> and <see cref="IGeocodeProviderResponse"/>.
        /// </summary>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// The <see cref="ILocation"/>.
        /// </returns>
        public ILocation BuildLocation(IAddress address, IGeocodeProviderResponse response)
        {
            var definitionFactory = new LocationTypeDefinitionFactory();
            var def = definitionFactory.GetEmptyDefaultLocationTypeDefinition();

            def.Fields.Address1().Value = address.Address1;
            def.Fields.Address2().Value = address.Address2;
            def.Fields.Locality().Value = address.Locality;
            def.Fields.Region().Value = address.Region;
            def.Fields.PostalCode().Value = address.PostalCode;
            def.Fields.CountryCode().Value = address.CountryCode;

            var location = new Location(def.Fields)
                               {
                                   LocationTypeKey = def.Key,
                                   GeocodeStatus = response.Status
                               };

            if (response.Status != GeocodeStatus.Ok) return location;
            if (!response.Results.Any()) return location;

            var result = response.Results.First();

            location.Coordinate = new Coordinate() { Latitude = result.Latitude, Longitude = result.Longitude };
            location.Viewport = result.Viewport;

            return location;
        }

        /// <summary>
        /// The build location.
        /// </summary>
        /// <param name="dto">
        /// The dto.
        /// </param>
        /// <returns>
        /// The <see cref="ILocation"/>.
        /// </returns>
        public ILocation BuildLocation(LocationDto dto)
        {
            return new Location(new CustomFieldsCollection(dto.FieldValues))
                {
                    Key = dto.Key,
                    Name = dto.Name,
                    LocationTypeKey = dto.LocationTypeKey,
                    Coordinate = _geography.GetCoordinateFromStPointText(dto.Coordinate),
                    Viewport = _geography.GetViewportFromStLinestringText(dto.Viewport),
                    GeocodeStatus = (GeocodeStatus)Enum.Parse(typeof(GeocodeStatus), dto.GeocodeStatus),
                    UpdateDate = dto.UpdateDate,
                    CreateDate = dto.CreateDate
                };
        }

        /// <summary>
        /// The build DTO.
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        /// <returns>
        /// The <see cref="LocationDto"/>.
        /// </returns>
        public LocationDto BuildDto(ILocation location)
        {
            return new LocationDto()
                {
                    Key = location.Key,
                    Name = location.Name,
                    LocationTypeKey = location.LocationTypeKey,
                    Coordinate = _geography.GetStPointText(location.Coordinate),
                    Viewport = _geography.GetStLineString(location.Viewport),
                    GeocodeStatus = location.GeocodeStatus.ToString(),   
                    FieldValues    = location.Fields.SerializeAsJson(),
                    UpdateDate = location.UpdateDate,
                    CreateDate = location.CreateDate
                };
        }
    }
}