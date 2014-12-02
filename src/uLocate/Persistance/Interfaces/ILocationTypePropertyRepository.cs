using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Persistance
{
    using uLocate.Models;

    /// <summary>
    /// LocationType Repository Interface 
    /// (following pattern: http://www.remondo.net/repository-pattern-example-csharp/)
    /// </summary>
    internal interface ILocationTypePropertyRepository
    {
        void Insert(LocationTypeProperty LocationTypeEntity);

        void Delete(LocationTypeProperty LocationTypeEntity);

        void Update(LocationTypeProperty LocationTypeEntity);

        LocationTypeProperty GetById(int Id);

        IEnumerable<LocationTypeProperty> GetById(int[] Ids);

        IEnumerable<LocationTypeProperty> GetAll();

        IEnumerable<LocationTypeProperty> GetByLocationType(int LocationTypeId);
    }
}
