using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uLocate.IO
{
    using uLocate.Models;
    using uLocate.Persistance;

    class DynamicLocationFlat
    {

        #region Custom Class Definition

        


        internal static string GetDynamicClass(Guid LocationTypeKey)
        {
            var classString = new StringBuilder();

            //Default props
            classString.Append(
                @"	[DelimitedRecord("","")]
                    [IgnoreEmptyLines()]
                    [IgnoreFirst(1)] 
                    public class LocationFlat
                    {
                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string LocationName;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Address1;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Address2;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Locality;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Region;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string PostalCode;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string CountryCode;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string PhoneNumber;

                        [FieldNullValue(typeof(string), """")]
                        [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                        public string Email;

                        [FieldNullValue(typeof(string), ""0"")]
                        public double Longitude;

                        [FieldNullValue(typeof(string), ""0"")]
                        public double Latitude;

			");

            var AllLtProps = Repositories.LocationTypePropertyRepo.GetByLocationType(Constants.DefaultLocationTypeKey).ToList();

            //Custom props
            if (LocationTypeKey != Constants.DefaultLocationTypeKey)
            {
                var CustomLtProps = Repositories.LocationTypePropertyRepo.GetByLocationType(LocationTypeKey);

                AllLtProps.AddRange(CustomLtProps);

                //Property Definitions
                foreach (var customProp in CustomLtProps.OrderBy(p => p.SortOrder))
                {
                    string fhProperty = FormatCustomProperty(customProp);
                    classString.Append(fhProperty);
                }
            }

            //Add Custom Get Prop & Set Prop methods
            string fhGetMethods = GenerateGetPropertyMethods(AllLtProps);
            classString.Append(fhGetMethods);

            string fhSetMethods = GenerateSetPropertyMethods(AllLtProps);
            classString.Append(fhSetMethods);


            //Close the class
            classString.Append("}");

            return classString.ToString();
        }

        private static string GenerateGetPropertyMethods(IEnumerable<LocationTypeProperty> CustomProps)
        {
            var classMethod = new StringBuilder();

            classMethod.Append(
            @" public string GetProperty(string Alias)
                {
                    switch (Alias)
                    {

            ");

            foreach (var prop in CustomProps)
            {
                switch (prop.DataType.DatabaseType)
                {
                    case CmsDataType.DbType.Ntext:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0};
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Nvarchar:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0};
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Integer:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0}.ToString();
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Date:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                return this.{0}.ToString();
                                break;
                                ", prop.Alias));
                        break;
                }
            }

            classMethod.Append(
            @"
                        default:
                            return """";
                    }
                }
            ");

            return classMethod.ToString();
        }

        private static string GenerateSetPropertyMethods(IEnumerable<LocationTypeProperty> CustomProps)
        {
            var classMethod = new StringBuilder();

            classMethod.Append(
            @" public void SetProperty(string Alias, object Data)
                {
                    switch (Alias)
                    {
            ");

            foreach (var prop in CustomProps)
            {
                switch (prop.DataType.DatabaseType)
                {
                    case CmsDataType.DbType.Ntext:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                this.{0} = Data.ToString();
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Nvarchar:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                this.{0} = Data.ToString();
                                break;
                                ", prop.Alias));
                        break;
                    case CmsDataType.DbType.Integer:
                        if (prop.DataType.PropertyEditorAlias == "Umbraco.TrueFalse")
                        {
                            classMethod.Append(string.Format(@"
                            case ""{0}"":
                                this.{0} = System.Convert.ToBoolean(Data);
                                break;
                                ", prop.Alias));
                        }
                        else
                        {
                            classMethod.Append(string.Format(@"
                            case ""{0}"":
                                this.{0} = System.Convert.ToInt32(Data);
                                break;
                                ", prop.Alias));
                        }
                        break;
                    case CmsDataType.DbType.Date:
                        classMethod.Append(string.Format(@"
                            case ""{0}"":
                                this.{0} = System.Convert.ToDateTime(Data);
                                break;
                                ", prop.Alias));
                        break;
                }
            }

            classMethod.Append(
            @"
                    }
                }
            ");

            return classMethod.ToString();
        }

        //        private static string GenerateCustomDateMethods(IEnumerable<LocationTypeProperty> CustomProps)
        //        {
        //            var classMethod = new StringBuilder();

        //            classMethod.Append(
        //            @" public DateTime GetProperty(string Alias)
        //                {
        //                    switch (Alias)
        //                    {
        //            ");

        //            foreach (var prop in CustomProps)
        //            {
        //                classMethod.Append(string.Format(
        //                @"
        //                            case ""{0}"":
        //                                return this.{0};
        //                                break;
        //                ", prop.Alias));
        //            }

        //            classMethod.Append(
        //            @"
        //                        default:
        //                            return DateTime.MinValue;
        //                    }
        //                }
        //            ");

        //            return classMethod.ToString();

        //        }

        //        private static string GenerateCustomIntegerMethods(IEnumerable<LocationTypeProperty> CustomProps)
        //        {
        //            var classMethod = new StringBuilder();

        //            classMethod.Append(
        //            @" public Integer GetProperty(string Alias)
        //                {
        //                    switch (Alias)
        //                    {
        //            ");

        //            foreach (var prop in CustomProps)
        //            {
        //                classMethod.Append(string.Format(
        //                @"
        //                            case ""{0}"":
        //                                return this.{0};
        //                                break;
        //                ", prop.Alias));
        //            }

        //            classMethod.Append(
        //            @"
        //                        default:
        //                            return 0;
        //                    }
        //                }
        //            ");

        //            return classMethod.ToString();
        //        }  

        private static string FormatCustomProperty(LocationTypeProperty CustomProperty)
        {
            var classProp = new StringBuilder();

            switch (CustomProperty.DataType.DatabaseType)
            {
                case CmsDataType.DbType.Date:
                    classProp.Append(
                        string.Format(
                        @"  [FieldConverter(ConverterKind.Date, ""MMddyyyy"")]
            	            public DateTime {0};
                        ",
                            CustomProperty.Alias));
                    break;
                case CmsDataType.DbType.Integer:
                    if (CustomProperty.DataType.PropertyEditorAlias == "Umbraco.TrueFalse")
                    {
                        classProp.Append(
                        string.Format(
                        @"  [FieldNullValue(typeof(bool), ""False"")]
                            [FieldConverter(ConverterKind.Boolean)]
                                                public bool {0};
                                            ",
                            CustomProperty.Alias));
                    }
                    else
                    {
                        classProp.Append(
                        string.Format(
                        @"  [FieldNullValue(typeof(int), ""0"")]
                                                public int {0};
                                            ",
                            CustomProperty.Alias));
                    }
                    break;
                default:
                    classProp.Append(
                        string.Format(
                        @"  [FieldNullValue(typeof(string), """")]
                            [FieldQuoted('""', QuoteMode.OptionalForBoth)]
                            public string {0};
                                            ",
                            CustomProperty.Alias));
                    break;
            }

            return classProp.ToString();
        }

        #endregion

    }


}
