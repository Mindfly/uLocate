namespace uLocate.Persistance
{
    using System.Collections.Generic;

    using uLocate.Models;

    /// <summary>
    /// LocationType Repository Interface 
    /// (following pattern: http://www.remondo.net/repository-pattern-example-csharp/)
    /// </summary>
    internal interface ILocationTypeRepository
    {
        void Insert(LocationType LocationTypeEntity);
        void Delete(LocationType LocationTypeEntity);
        void Update(LocationType LocationTypeEntity);
        LocationType GetById(int Id);
        IEnumerable<LocationType> GetById(int[] Ids);
        IEnumerable<LocationType> GetAll();
    }
}