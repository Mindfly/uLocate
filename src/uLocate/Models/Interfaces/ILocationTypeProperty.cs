using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Models
{
    public interface ILocationTypeProperty : IEntity
    {
        int Id { get; set; }

        string Alias { get; set; }
        
        string Name { get; set; }
        
        int UmbracoDataTypeId { get; set; }

        int LocationTypeId { get; set; }

        int SortOrder { get; set; }

        DateTime CreateDate { get; set; }

        DateTime UpdateDate { get; set; }
    }
}
