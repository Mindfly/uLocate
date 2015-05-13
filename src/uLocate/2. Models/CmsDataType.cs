namespace uLocate.Models
{
    public class CmsDataType
    {
        public enum DbType
        {
            Ntext,
            Nvarchar,
            Integer,
            Date,
            Unknown
        }

        public int Key { get; set; }

        public int DataTypeId { get; set; }

        public string PropertyEditorAlias { get; set; }

        public string DatabaseTypeString { get; set; }

        public DbType DatabaseType
        {
            get
            {
               return StringToDbType(this.DatabaseTypeString);
            }
        }

        private DbType StringToDbType(string DatabaseTypeText)
        {
            switch (DatabaseTypeText)
            {
                case Constants.DbNtext:
                    return DbType.Ntext;
                    break;
                case Constants.DbNvarchar:
                    return DbType.Nvarchar;
                    break;
                case Constants.DbInteger:
                    return DbType.Integer;
                    break;
                case Constants.DbDate:
                    return DbType.Date;
                    break;
                default:
                    return DbType.Unknown;
            }
        }

    }
}
