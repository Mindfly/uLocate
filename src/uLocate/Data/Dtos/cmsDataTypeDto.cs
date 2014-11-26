namespace uLocate.Data
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    [TableName("cmsDataType")]
	[PrimaryKey("pk")]
	[ExplicitColumns] 
    internal class cmsDataTypeDto
    {

        [Column("pk")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Key { get; set; }

        [Column("nodeId")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int DataTypeId { get; set; }

        [Column("propertyEditorAlias")]
        [Length(255)]
        public string PropertyEditorAlias { get; set; } 

        [Column("dbType")]
        [Length(50)]
        public string DatabaseType { get; set; } 

    }
}

