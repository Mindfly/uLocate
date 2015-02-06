namespace uLocate.Data
{
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;


    internal class CmsDataTypePreValues
    {
        public int Id { get; set; }
        
        public int DataTypeNodeId { get; set; }

        public string Value { get; set; }
        
        public int SortOrder { get; set; }

        public string Alias { get; set; } 

    }
}

