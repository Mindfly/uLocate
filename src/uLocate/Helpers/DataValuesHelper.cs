using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Helpers
{
    using uLocate.Data;
    using uLocate.Persistance;

    using Umbraco.Core.Persistence;

    internal static class DataValuesHelper
    {
        public static string GetPreValueId(int DataTypeId, string PrevalueText)
        {
            var sql = new Sql();
            sql
                .Select("*")
                .From<cmsDataTypePreValuesDto>()
                .Where<cmsDataTypePreValuesDto>(n => n.DataTypeNodeId == DataTypeId);

            var allDataTypePrevals = Repositories.ThisDb.Fetch<cmsDataTypePreValuesDto>(sql);
            var match = allDataTypePrevals.Where(n => n.Value == PrevalueText).FirstOrDefault();
            if (match != null)
            {
                return match.Id.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
