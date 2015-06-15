namespace uLocate.Data
{
    using System;

    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    [TableName("uLocate_PostalCodeCounter")]
    [ExplicitColumns]
    internal class PostalCodeCounterDto
    {
        [Column("PostalCode")]
        [Length(150)]
        public string PostalCode { get; set; }

        [Column("LocationCount")]
        [Length(150)]
        public string LocationCount { get; set; }

        public PostalCodeCounterDto()
        {
            PostalCode = string.Empty;
            LocationCount = "0";
        }
    }

}