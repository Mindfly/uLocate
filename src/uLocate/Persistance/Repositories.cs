using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Persistance
{
    using uLocate.Models;

    using Umbraco.Core;
    using Umbraco.Core.Persistence;

    static class Repositories
    {
        public static UmbracoDatabase ThisDb = ApplicationContext.Current.DatabaseContext.Database;
        public static LocationTypeRepository LocationTypeRepo = new LocationTypeRepository(ThisDb, ApplicationContext.Current.ApplicationCache.RuntimeCache);
        public static LocationTypePropertyRepository LocationTypePropertyRepo = new LocationTypePropertyRepository(ThisDb, ApplicationContext.Current.ApplicationCache.RuntimeCache);


        public static LocationTypeProperty CreateLocationTypeProp(string Alias, string DisplayName, int DataTypeId)
        {
            var NewProp = new LocationTypeProperty();
            NewProp.Alias = "BusinessWebsite";

            return NewProp;
        }
        
    }
}
