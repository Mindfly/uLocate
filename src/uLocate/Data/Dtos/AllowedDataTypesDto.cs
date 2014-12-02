
		using System;
		using Umbraco.Core.Persistence;
		using Umbraco.Core.Persistence.DatabaseAnnotations; 

namespace uLocate.Data
{
    /// <summary>
    /// The allowed data types dto.
    /// </summary>
    [TableName("uLocate_AllowedDataTypes")]
    [PrimaryKey("Id")]
    [ExplicitColumns] 
    internal class AllowedDataTypesDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the related DataType (from the umbraco DataTypes)
        /// </summary>
        [Column("DataTypeId")]
        [ForeignKey(typeof(cmsDataTypeDto), Name = "FK_uLocateAllowedDataTypes_cmsDataType", Column = "nodeId")]
        public int UmbracoDataTypeId { get; set; }
    }
}
