using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Models
{
    public class PagedLocations
    {

        public IEnumerable<JsonLocation> Locations { get; set; }

        public long PageNum { get; set; }

        public long ItemsPerPage { get; set; }

        public long TotalItems { get; set; }

    }
}
