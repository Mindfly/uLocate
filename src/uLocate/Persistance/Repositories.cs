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
        public static LocationPropertyDataRepository LocationPropertyDataRepo = new LocationPropertyDataRepository(ThisDb, ApplicationContext.Current.ApplicationCache.RuntimeCache);
        public static LocationRepository LocationRepo = new LocationRepository(ThisDb, ApplicationContext.Current.ApplicationCache.RuntimeCache);


        //public static LocationTypeProperty CreateLocationTypeProp( string Alias, string DisplayName, int DataTypeId, int SortOrder = 0)
        //{//int LocTypeId,
        //    var NewProp = new LocationTypeProperty();
        //    //NewProp.LocationTypeId = LocTypeId;
        //    NewProp.Alias = Alias;
        //    NewProp.Name = DisplayName;
        //    NewProp.DataTypeId = DataTypeId;
        //    NewProp.SortOrder = SortOrder;
            
        //    return NewProp;
        //}
        
    }
}
