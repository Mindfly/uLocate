namespace uLocate.Data
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    [TableName("cmsDataTypePreValues")]
	[PrimaryKey("id")]
	[ExplicitColumns]
    internal class cmsDataTypePreValuesDto
    {

        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("datatypeNodeId")]
        public int DataTypeNodeId { get; set; }

        [Column("value")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Value { get; set; }

        [Column("sortorder")]
        public int SortOrder { get; set; }

        [Column("alias")]
        [Length(50)]
        [NullSetting(NullSetting = NullSettings.Null)]
        public string Alias { get; set; } 

    }
}

