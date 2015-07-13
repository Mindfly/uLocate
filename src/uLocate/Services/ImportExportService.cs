using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.Services
{
    using uLocate.Models;

    public class ImportExportService
    {
        #region Info

        public string GetListofColumnHeaders(Guid LocationTypeKey)
        {
            return uLocate.IO.Export.GetListofColumnHeaders(LocationTypeKey);
        }

        #endregion

        #region Import

        public StatusMessage ImportLocationsCSV(string FilePath, Guid? LocationTypeKey = null, bool SkipGeocoding = false)
        {
            return uLocate.IO.Import.LocationsCSV(FilePath, LocationTypeKey, SkipGeocoding);
        }

        public StatusMessage ImportLocationsCSV(string FilePath, Guid LocationTypeKey)
        {
            return uLocate.IO.Import.LocationsCSV(FilePath, LocationTypeKey);
        }

        public StatusMessage ImportLocationsCSV(string FilePath)
        {
            return uLocate.IO.Import.LocationsCSV(FilePath);
        }

        #endregion

        #region Export

        public StatusMessage ExportAllLocations(Guid LocationTypeKey)
        {
            return uLocate.IO.Export.ExportAllLocations(LocationTypeKey);
        }

        #endregion
    }
}
