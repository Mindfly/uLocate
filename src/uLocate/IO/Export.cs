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
    using Newtonsoft.Json;
    using uLocate.Models;
    using uLocate.Persistance;
    using Umbraco.Core.Logging;

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

            string errorMsg = "";
            string errorData = "";
            int errorIndex = 0;

            try
            {
                // Dynamic class definition
                errorIndex = 1;
                errorMsg = "Error while creating Dynamic Class for Export. Class Definition:";

                string dynamicClassDef = DynamicLocationFlat.GetDynamicClass(LocTypeKey);

                errorData = dynamicClassDef;
                
                Type DynamicLocation = ClassBuilder.ClassFromString(dynamicClassDef);


                // Using http://filehelpers.sourceforge.net/ to get dynamic headers
                errorIndex = 2;
                errorMsg = "Error while using FileHelperEngine to get headers";
                
                FileHelperEngine fhEngine = new FileHelperEngine(DynamicLocation);
                string dynamicLocationHeaders = fhEngine.GetFileHeader();
                var headers = dynamicLocationHeaders.Split(',');
                errorData = "dynamicLocationHeaders = " + dynamicLocationHeaders;


                //Prepare to get a list of dynamic objects
                errorIndex = 3;
                errorMsg = "Error while converting Locations to dynamic objects";
                
                MethodInfo setMethod = DynamicLocation.GetMethod("SetProperty");

                var locationsExport = CreateListOfType(DynamicLocation);
                //Type listType = typeof(List<>).MakeGenericType(new[] { DynamicLocation });
                //IList locations = (IList)Activator.CreateInstance(listType);


                var allLocationsOfType = Repositories.LocationRepo.GetByType(LocTypeKey);

                foreach (EditableLocation loc in allLocationsOfType)
                {
                    dynamic dynLoc = new[] { DynamicLocation };
                    //var dynLoc = CreateType(DynamicLocation); 
                    //dynamic dynLoc = typeof(DynamicLocation);
                    //errorData = "dynLoc.GetType()=" + dynLoc.GetType();

                    //errorData = JsonConvert.SerializeObject(dynLoc);
                    foreach (string prop in headers)
                    {
                        var currentProperty = loc.GetType().GetProperty(prop);

                        if (currentProperty != null)
                        {
                            Type type = DynamicLocation; //Throws System.Reflection.TargetException: 'Object does not match target type'
                            PropertyInfo tempProperty = type.GetProperty(prop);
                            tempProperty.SetValue(dynLoc, currentProperty.GetValue(prop));
                        }

                        if (prop == "LocationName")
                        {
                            object dataObj = loc.Name;
                            dynLoc.SetProperty(prop, dataObj);
                            //setMethod.Invoke(dynLoc, new[] { prop, dataObj });
                        }
                        else
                        {
                            var propData = loc.PropertyData.Where(n => n.PropertyAlias == prop).FirstOrDefault();
                            //setMethod.Invoke(dynLoc, new object[] { prop, propData.Value.ValueObject });
                            dynLoc.SetProperty(prop, propData.Value.ValueObject);
                        }

                    }

                    locationsExport.Add(dynLoc);
                }

                fhEngine.WriteFile(file, locationsExport.ToArray());

            }
            catch (Exception exDynamicClass)
            {
                string msg = "";

                switch (errorIndex)
                {
                    //case 1:
                    //    msg = string.Format("Error while creating Dynamic Class for Export. Class Definition:{0}{1}", Environment.NewLine, errorData);
                    //    break;
                    case 2:
                        msg = string.Format("Error while using FileHelperEngine:{0}{1}", Environment.NewLine, errorData);
                        break;
                    default:
                        msg = string.Format("{0}{1}{2}",errorMsg, Environment.NewLine, errorData);
                        break;
                }

                LogHelper.Error<Export>(msg, exDynamicClass);
            }

            return Msg;
        }

        static List<T> CreateListOfType<T>(T obj)
        {
            return new List<T>();
        }

        //static T CreateObjectOfType<T>(T obj)
        //{
        //    return <T>();
        //}
    }
}

