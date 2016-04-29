using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FileHelpers;
using FileHelpers.Dynamic;
using umbraco;

namespace uLocate.IO
{
    using System;

    using Models;
    using Persistance;

    internal class Export 
    {
        private static readonly Object ThisLock = new Object();
        private const String Comma = ",";
        private const String ExportDirectoryName = "Export";
        private static readonly IList<String> ExportCsvFileHeaders = new List<String>()
        {
           "LocationName", "Latitude", "Longitude", "Address1", "Address2", "Phone", "Email", "PostalCode", "CountryCode", "Locality", "Region"
        };


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
            var dataTypeDefinitions = umbraco.cms.businesslogic.datatype.DataTypeDefinition.GetAll().ToList();
            
            var locationTypes = new List<umbraco.cms.businesslogic.datatype.PreValue>();
            foreach (var dataTypeDefinition in dataTypeDefinitions)
            {
                locationTypes.AddRange(uQuery.GetPreValues(dataTypeDefinition.Id));
            }

            var locationTypeKey = LocationTypeKey ?? Constants.DefaultLocationTypeKey;
            var locationTypeName = Repositories.LocationTypeRepo.GetByKey(locationTypeKey).Name;
            var locations = Repositories.LocationRepo.GetByType(locationTypeKey).ToList();

            var directoryPathForExport = GetDirectoryPathForExport(ExportDirectoryName);
            var fileNameForExport = String.Format("uLocateExport-{0} {1}.csv", locationTypeName, GetCurrentDateTime());
            var filePathForExport = String.Format("/{0}/{1}", ExportDirectoryName, fileNameForExport);
            var serverFilPathForExport = String.Format("{0}/{1}", directoryPathForExport, fileNameForExport);
            lock (ThisLock)
            {
                using (var fileStream = new FileStream(serverFilPathForExport, FileMode.Create, FileAccess.Write))
                {
                    using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        var csvHeaderLine = new StringBuilder();
                        foreach (var exportCsvFileHeader in ExportCsvFileHeaders)
                        {
                            csvHeaderLine.Append(exportCsvFileHeader).Append(Comma);
                        }

                        if (locations.Any() && locations.First().PropertyData.Any(item => !item.PropertyAttributes.IsDefaultProp))
                        {
                            foreach (var propertyItem in locations.First().PropertyData.Where(item => !item.PropertyAttributes.IsDefaultProp)
                                )
                            {
                                csvHeaderLine.Append(propertyItem.PropertyAlias).Append(Comma);
                            }
                        }

                        streamWriter.WriteLine(csvHeaderLine);

                        foreach (var location in locations)
                        {
                            var csvLine = new StringBuilder();
                            csvLine.Append(ToCsvString(location.Name)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Latitude.ToString())).Append(Comma);
                            csvLine.Append(ToCsvString(location.Longitude.ToString())).Append(Comma);
                            csvLine.Append(ToCsvString(location.Address.Address1)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Address.Address2)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Phone)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Email)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Address.PostalCode)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Address.CountryCode)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Address.Locality)).Append(Comma);
                            csvLine.Append(ToCsvString(location.Address.Region)).Append(Comma);

                            if (location.PropertyData.Any(item => item.PropertyAttributes.IsDefaultProp))
                            {
                                var existingPropertyDataAliases = new List<String>();
                                foreach (var propertyItem in location.PropertyData.Where(item => !item.PropertyAttributes.IsDefaultProp))
                                {
                                    if (!existingPropertyDataAliases.Contains(propertyItem.PropertyAlias))
                                    {
                                        var locationTypesById = locationTypes.FirstOrDefault(item => item.Id.ToString().Equals(propertyItem.Value.ToString()));
                                        if (locationTypesById != null)
                                        {
                                            csvLine.Append(ToCsvString(locationTypesById.Value)).Append(Comma);
                                        }
                                        existingPropertyDataAliases.Add(propertyItem.PropertyAlias);
                                    }
                                }
                            }

                            streamWriter.WriteLine(csvLine.ToString());
                        }
                    }
                }
            }

            return new StatusMessage()
            {
                ObjectName = filePathForExport,
                Success = true
            };
        }

        private static String GetCurrentDateTime()
        {
            var formattedMonth = DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString();
            var formattedDay = DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString();

            return String.Format("{0}-{1}-{2}", DateTime.Now.Year, formattedMonth, formattedDay);
        }

        public static String ToCsvString(String input)
        {
            if (input.Contains("\""))
            {
                input = input.Replace("\"", "\"\"");
            }
            if (input.Contains(","))
            {
                input = string.Format("\"{0}\"", input);
            }
            if (input.Contains(Environment.NewLine) || input.Contains("\n"))
            {
                input = string.Format("\"{0}\"", input);
            }
            return input;
        }

        private static String GetDirectoryPathForExport(String directoryName)
        {
            var path = HttpContext.Current.Server.MapPath(String.Format("~/{0}", directoryName));
            CreateExportDirectory(path);
            return path;
        }

        private static void CreateExportDirectory(String path)
        {
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
    }
}

