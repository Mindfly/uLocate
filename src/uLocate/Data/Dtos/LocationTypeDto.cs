namespace uLocate.Data
{
    using System;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    /// <summary>
    /// The location type dto.
    /// </summary>
    [TableName("uLocate_LocationType")]
    [PrimaryKey("Id")]
    [ExplicitColumns] 
    internal class LocationTypeDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Location name.
        /// </summary>
        [Column("Name")]
        [Length(150)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        [Column("CreateDate")]
        [Constraint(Default = "getdate()")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        [Column("UpdateDate")]
        [Constraint(Default = "getdate()")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationTypeDto"/> class and sets some default values.
        /// </summary>
        public LocationTypeDto()
        {
            UpdateDate = DateTime.Now;
            CreateDate = DateTime.Now;
        }
    }
}
