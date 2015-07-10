using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Models
{
    public class SearchedLocation
    {
        public int ExamineNodeId { get; set; }
        public float SearchScore { get; set; }
        public IndexedLocation IndexedLocation { get; set; }
    }
    
}
