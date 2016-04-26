using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace uLocate.IO
{
    using System;

    using FileHelpers;
    using FileHelpers.Dynamic;

    using Mindfly;

    using Models;
    using Persistance;

    internal class Export
    {
        internal static string GetListofColumnHeaders(Guid? LocationTypeKey)
        {
            Guid LocTypeKey;
            if (LocationTypeKey == null)
            {
                LocTypeKey = Constants.DefaultLocationTypeKey;
            }
            else
            {
                LocTypeKey = (Guid)LocationTypeKey;
            }

            // Dynamic class definition
            string dynamicClassDef = DynamicLocationFlat.GetDynamicClass(LocTypeKey);

            Type t = ClassBuilder.ClassFromString(dynamicClassDef);

            // Using http://filehelpers.sourceforge.net/ to deal with dynamic model
            FileHelperEngine fhEngine = new FileHelperEngine(t);

            return fhEngine.GetFileHeader();
        }
        internal static StatusMessage ExportAllLocations(Guid? LocationTypeKey)
        {
            var guid = LocationTypeKey.HasValue ? LocationTypeKey.Value : Constants.DefaultLocationTypeKey;
            var name = Repositories.LocationTypeRepo.GetByKey(guid).Name;
            var fileHelperEngine = new FileHelperEngine<LocationFlat>();
            var strArray = fileHelperEngine.GetFileHeader().Split(',');
            var byType = Repositories.LocationRepo.GetByType(guid);
            var list = new List<LocationFlat>();
            var locationFlat1 = new LocationFlat()
            {
                Address1 = "Address1",
                Address2 = "Address2",
                CountryCode = "CountryCode",
                Email = "Email",
                Latitude = "Latitude",
                Locality = "Locality",
                LocationName = "LocationName",
                Longitude = "Longitude",
                PhoneNumber = "PhoneNumber",
                PostalCode = "PostalCode",
                Region = "Region"
            };
            list.Add(locationFlat1);
            foreach (EditableLocation editableLocation in byType)
            {
                LocationFlat locationFlat2 = new LocationFlat();
                foreach (string str in strArray)
                {
                    string prop = str;
                    switch (prop)
                    {
                        case "LocationName":
                            object data1 = (object)editableLocation.Name;
                            locationFlat2.SetProperty(prop, data1);
                            break;
                        case "Longitude":
                            object data2 = (object)editableLocation.Longitude;
                            locationFlat2.SetProperty(prop, data2);
                            break;
                        case "Latitude":
                            object data3 = (object)editableLocation.Latitude;
                            locationFlat2.SetProperty(prop, data3);
                            break;
                        default:
                            LocationPropertyData locationPropertyData = editableLocation.PropertyData.FirstOrDefault(n => n.PropertyAlias == prop);
                            if (locationPropertyData != null)
                            {
                                locationFlat2.SetProperty(prop, locationPropertyData.Value.ValueObject);
                                break;
                            }
                            break;
                    }
                }
                list.Add(locationFlat2);
            }
            string exportDirectoryPath = GetExportDirectoryPath("Export");
            string str1 = string.Format("uLocateExport-{0}.csv", (object)Extensions.MakeCodeSafe(name, "-"));
            string str2 = string.Format("/{0}/{1}", (object)"Export", (object)str1);
            string fileName = string.Format("{0}//{1}", (object)exportDirectoryPath, (object)str1);
            fileHelperEngine.WriteFile(fileName, (IEnumerable<LocationFlat>)list.ToArray());
            return new StatusMessage()
            {
                ObjectName = str2,
                Success = true
            };
        }

        private static string GetExportDirectoryPath(string directoryName)
        {
            string path = HttpContext.Current.Server.MapPath(string.Format("~/{0}", (object)directoryName));
            CreateExportDirectory(path);
            return path;
        }

        private static void CreateExportDirectory(string path)
        {
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
    }
}

