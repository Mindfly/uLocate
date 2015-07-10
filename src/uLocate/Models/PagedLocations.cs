using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Models
{
    public class PagedLocations
    {
        public long ItemsPerPage { get; set; }

        public long TotalItems { get; set; }

        public long TotalPages { get; set; }

        public IEnumerable<PageOfLocations> Pages { get; set; }

    }

    public class PageOfLocations
    {
        public long PageNum { get; set; }

        public long ItemsPerPage { get; set; }

        public long TotalItems { get; set; }

        public long TotalPages { get; set; }

        public IEnumerable<IndexedLocation> Locations { get; set; }
    }
}
