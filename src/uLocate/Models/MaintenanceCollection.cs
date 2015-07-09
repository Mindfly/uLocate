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
            this.Locations = new List<Location>();
            this.JsonLocations = new List<JsonLocation>();
        }

        public string Title { get; set; }

        public IEnumerable<JsonLocation> JsonLocations { get; set; }

        public IEnumerable<Location> Locations { get; set; }

        public int Count
        {
            get
            {
                if (this.JsonLocations.Any())
                {
                    return JsonLocations.Count();
                }
                else if (this.Locations.Any())
                {
                    return Locations.Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        public void SyncLocationLists()
        {
            if (this.Locations != null & this.Locations.Any())
            {
                JsonLocations = uLocate.Helpers.Convert.LocationsToJsonLocations(this.Locations);
            }
            else if (this.JsonLocations != null)
            {
                if (this.JsonLocations.Any())
                {
                    var listLocs = new List<Location>();

                    foreach (var jsonLocation in JsonLocations)
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
            this.Locations = new List<Location>();
        }
    }
}
