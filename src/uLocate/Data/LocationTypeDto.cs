namespace uLocate.Data
{
    using System;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    [TableName("uLocate_LocationType")]
    [PrimaryKey("id")]
    [ExplicitColumns] 
    class LocationTypeDto
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("Alias")]
        [Length(150)]
        public string Alias { get; set; } 

    }
}
