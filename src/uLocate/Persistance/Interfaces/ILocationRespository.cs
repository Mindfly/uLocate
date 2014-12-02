namespace uLocate.Persistance
{
    using System;

    using uLocate.Models;

    /// <summary>
    /// LocationType Repository Interface 
    /// (following pattern: http://www.remondo.net/repository-pattern-example-csharp/)
    /// </summary>
    internal interface ILocationRespository
    {
        void Insert(Location LocationEntity);
        void Delete(Location LocationEntity);
        Location GetByKey(Guid Key);
    }
}