namespace uLocate.IO
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FileHelpers;
    using FileHelpers.Dynamic;

    using Mindfly;

    using uLocate.Models;
    using uLocate.Persistance;

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


        //TODO: Heather - Fix this!
        internal static StatusMessage ExportAllLocations(Guid? LocationTypeKey)
        {
            //string FilePath


            Guid LocTypeKey;
            if (LocationTypeKey == null)
            {
                LocTypeKey = Constants.DefaultLocationTypeKey;
            }
            else
            {
                LocTypeKey = (Guid)LocationTypeKey;
            }

            string LocationTypeName = Repositories.LocationTypeRepo.GetByKey(LocTypeKey).Name;

            string file = string.Format("~/uLocateExport-{0}.csv", LocationTypeName.MakeCodeSafe());

            StatusMessage Msg = new StatusMessage();

            Msg.ObjectName = file;

            // Dynamic class definition
            string dynamicClassDef = DynamicLocationFlat.GetDynamicClass(LocTypeKey);
            Type DynamicLocation = ClassBuilder.ClassFromString(dynamicClassDef);

            // Using http://filehelpers.sourceforge.net/ to write csv 
            FileHelperEngine fhEngine = new FileHelperEngine(DynamicLocation);

            string dynamicLocationHeaders = fhEngine.GetFileHeader(); 
            var headers = dynamicLocationHeaders.Split(',');
            MethodInfo setMethod = DynamicLocation.GetMethod("SetProperty");

            var locationsExport = CreateListOfType(DynamicLocation);
            //Type listType = typeof(List<>).MakeGenericType(new[] { DynamicLocation });
            //IList locations = (IList)Activator.CreateInstance(listType);

            var allLocationsOfType = Repositories.LocationRepo.GetByType(LocTypeKey);

            foreach (EditableLocation loc in allLocationsOfType)
            {
                dynamic dynLoc = new[] { DynamicLocation };

                foreach (string prop in headers)
                {
                    if (prop == "LocationName")
                    {
                        object dataObj = loc.Name;
                        setMethod.Invoke(dynLoc, new object[] { prop, dataObj });
                    }
                    else
                    {
                        var propData = loc.PropertyData.Where(n => n.PropertyAlias == prop).FirstOrDefault();
                        setMethod.Invoke(dynLoc, new object[] { prop, propData.Value.ValueObject });
                    }

                }

                locationsExport.Add(dynLoc);
            }

            fhEngine.WriteFile(file, locationsExport.ToArray());  

            return Msg;
        }

        static List<T> CreateListOfType<T>(T obj)
        {
            return new List<T>();
        }

    }
}

