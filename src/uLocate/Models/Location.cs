namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a Location
    /// </summary>
    public class Location : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> class.
        /// </summary>
        public Location()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
            //this.Key = Guid.NewGuid();
            this.PropertyData = new List<LocationPropertyData>();
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public override Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location type key.
        /// </summary>
        public Guid LocationTypeKey { get; set; }

        /// <summary>
        /// Gets or sets the geocode status.
        /// </summary>
        public GeocodeStatus GeocodeStatus { get; set; }

        /// <summary>
        /// Gets or sets the viewport.
        /// </summary>
        public Viewport Viewport { get; set; }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>
        public ICoordinate Coordinate { get; set; }

        /// <summary>
        /// Gets the custom fields collection.
        /// </summary>
        public IEnumerable<LocationPropertyData> PropertyData { get; internal set; }

        /// <summary>
        /// Gets or sets the location type.
        /// </summary>
        public LocationType LocationType { get; set; }



    }

}