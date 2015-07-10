using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Models
{
    using uLocate.Persistance;

    public class MaintenanceCollection
    {
        public MaintenanceCollection()
        {
            this.EditableLocations = new List<EditableLocation>();
            this.IndexedLocations = new List<IndexedLocation>();
        }

        public string Title { get; set; }

        public IEnumerable<IndexedLocation> IndexedLocations { get; set; }

        public IEnumerable<EditableLocation> EditableLocations { get; set; }

        public int Count
        {
            get
            {
                if (this.IndexedLocations.Any())
                {
                    return IndexedLocations.Count();
                }
                else if (this.EditableLocations.Any())
                {
                    return EditableLocations.Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        public void SyncLocationLists()
        {
            if (this.EditableLocations != null & this.EditableLocations.Any())
            {
                IndexedLocations = uLocate.Helpers.Convert.EditableLocationsToIndexedLocations(this.EditableLocations);
            }
            else if (this.IndexedLocations != null)
            {
                if (this.IndexedLocations.Any())
                {
                    var listLocs = new List<EditableLocation>();

                    foreach (var jsonLocation in IndexedLocations)
                    {
                        listLocs.Add(jsonLocation.ConvertToLocation());
                    }
                }
            }
        }

        public void ConvertToJsonLocationsOnly()
        {
            this.SyncLocationLists();
            //clear Locations
            this.EditableLocations = new List<EditableLocation>();
        }
    }
}
