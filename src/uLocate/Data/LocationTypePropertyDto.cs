using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations; 

namespace uLocate.Data
{
    [TableName("uLocate_LocationTypeProperty")]
    [PrimaryKey("Id")]
    [ExplicitColumns] 
    internal class LocationTypePropertyDto
    {

    }
}
