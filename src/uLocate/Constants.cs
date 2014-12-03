namespace uLocate
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using uLocate.Data;
    using uLocate.Models;

    using Umbraco.Core;

    /// <summary>
    /// uLocate constants
    /// </summary>
    public class Constants
    {
        #region General

        /// <summary>
        /// Gets the Umbraco Package Name
        /// </summary>
        public static string PackageName
        {
            get { return "uLocate"; }
        }

        ///// <summary>
        ///// The database connection info.
        ///// </summary>
        //public static class DatabaseConnectionInfo
        //{
        //    /// <summary>
        //    /// Gets the connection string.
        //    /// </summary>
        //    public static string ConnectionString
        //    {
        //        get { return ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString; }
        //    }

        //    /// <summary>
        //    /// Gets the provider name.
        //    /// </summary>
        //    public static string ProviderName
        //    {
        //        get { return ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ProviderName; }
        //    }
        //}


        /// <summary>
        /// Gets the world geodetic coordinate system SRID (spacial reference Id)
        /// </summary>
        public static int WorldGeodeticSystemSrid
        {
            get { return 4326; }
        }

        #endregion

        #region DataTypes & Property Editors

        /// <summary>
        /// The allowed data types dictionary.
        /// </summary>
        public static readonly Dictionary<string, int> AllowedDataTypesDictionary = new Dictionary<string, int>
        {
            // {"PropertyEditorAlias", DataTypeNodeId}
            { PropertyEditorAlias.TextBox, DataTypeId.TextBox }, 
            { PropertyEditorAlias.TextBoxMultiple, DataTypeId.TextBoxMultiple }, 
            { PropertyEditorAlias.TrueFalse, DataTypeId.TrueFalse }, 
            { PropertyEditorAlias.MultipleMediaPicker, DataTypeId.MultipleMediaPicker }, 
            { PropertyEditorAlias.MemberPicker, DataTypeId.MemberPicker }, 
            { PropertyEditorAlias.ContentPicker, DataTypeId.ContentPicker }, 
            { PropertyEditorAlias.CheckBoxList, DataTypeId.CheckBoxList }
        };

        /// <summary>
        /// Returns NodeIds for Umbraco DataTypes
        /// </summary>
        public static class DataTypeId
        {
            //TODO: Update to verify correct IDs from cmsDataType Table?

            /// <summary>
            /// Gets the text box datatype node id
            /// </summary>
            public static int TextBox
            {
                get
                {
                    return -88;
                }
            }

            /// <summary>
            /// Gets the text box multiple datatype node id
            /// </summary>
            public static int TextBoxMultiple
            {
                get
                {
                    return -89;
                }
            }

            /// <summary>
            /// Gets the true/false datatype node id
            /// </summary>
            public static int TrueFalse
            {
                get
                {
                    return -49;
                }
            }

            /// <summary>
            /// Gets the multiple media picker datatype node id
            /// </summary>
            public static int MultipleMediaPicker
            {
                get
                {
                    return 1045;
                }
            }

            /// <summary>
            /// Gets the member picker datatype node id
            /// </summary>
            public static int MemberPicker
            {
                get
                {
                    return 1036;
                }
            }

            /// <summary>
            /// Gets the content picker datatype node id
            /// </summary>
            public static int ContentPicker
            {
                get
                {
                    return 1034;
                }
            }

            /// <summary>
            /// Gets the check box list datatype node id
            /// </summary>
            public static int CheckBoxList
            {
                get
                {
                    return -43;
                }
            }

        }

        /// <summary>
        /// Property Editor Aliases (from Umbraco)
        /// </summary>
        public static class PropertyEditorAlias
        {
            /// <summary>
            /// Gets the check box list alias.
            /// </summary>
            public static string CheckBoxList
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.CheckBoxListAlias;
                }
            }

            /// <summary>
            /// Gets the content picker alias.
            /// </summary>
            public static string ContentPicker
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.ContentPickerAlias;
                }
            }

            /// <summary>
            /// Gets the drop down list alias.
            /// </summary>
            public static string DropDownList
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.DropDownListAlias;
                }
            }

            /// <summary>
            /// Gets the member picker.
            /// </summary>
            public static string MemberPicker
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.MemberPickerAlias;
                }
            }

            /// <summary>
            /// Gets the multiple media picker alias.
            /// </summary>
            public static string MultipleMediaPicker
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.MultipleMediaPickerAlias;
                }
            }

            /// <summary>
            /// Gets the text box alias.
            /// </summary>
            public static string TextBox
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.TextboxAlias;
                }
            }

            /// <summary>
            /// Gets the text box multiple.
            /// </summary>
            public static string TextBoxMultiple
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.TextboxMultipleAlias;
                }
            }

            /// <summary>
            /// Gets the true false.
            /// </summary>
            public static string TrueFalse
            {
                get
                {
                    return Umbraco.Core.Constants.PropertyEditors.TrueFalseAlias;
                }
            }

        }

        #endregion

        #region 'Default' Location Type

        ///// <summary>
        ///// Gets the default location type key
        ///// </summary>
        //public static Guid DefualtLocationTypeKey
        //{
        //    get
        //    {
        //        return "00EEC1F9-7152-4B3B-B8D5-5B813F86EB66".EncodeAsGuid();
        //    }
        //}

        /// <summary>
        /// Gets the "Default" location type id
        /// </summary>
        public static int DefaultLocationTypeId
        {
            get
            {
                //TODO: Update to check Db?
                return 1;
            }
        }

        internal static List<LocationTypeProperty> DefaultLocationTypeProperties
        {
            get
            {
                List<LocationTypeProperty> DefaultProps = new List<LocationTypeProperty>();

                DefaultProps.Add(new LocationTypeProperty { LocationTypeId = Constants.DefaultLocationTypeId, Alias = Constants.DefaultLocPropertyAlias.Address1, Name = "Address 1", DataTypeId = -88, SortOrder = 1 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeId = Constants.DefaultLocationTypeId, Alias = Constants.DefaultLocPropertyAlias.Address2, Name = "Address 2", DataTypeId = -88, SortOrder = 2 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeId = Constants.DefaultLocationTypeId, Alias = Constants.DefaultLocPropertyAlias.Region, Name = "State/Province", DataTypeId = -88, SortOrder = 3 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeId = Constants.DefaultLocationTypeId, Alias = Constants.DefaultLocPropertyAlias.PostalCode, Name = "Zip/Postal Code", DataTypeId = -88, SortOrder = 4 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeId = Constants.DefaultLocationTypeId, Alias = Constants.DefaultLocPropertyAlias.CountryCode, Name = "Country", DataTypeId = -88, SortOrder = 5 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeId = Constants.DefaultLocationTypeId, Alias = "PhoneNumber", Name = "Phone Number", DataTypeId = -88, SortOrder = 6 });

                return DefaultProps;
            }
        }




        /// <summary>
        /// Reserved 'default' custom field aliases.
        /// </summary>
        public static class DefaultLocPropertyAlias
        {
            /// <summary>
            /// Gets the address 1.
            /// </summary>
            public static string Address1
            {
                get
                {
                    return "Address1";
                }
            }

            /// <summary>
            /// Gets the address 2.
            /// </summary>
            public static string Address2
            {
                get
                {
                    return "Address2";
                }
            }

            /// <summary>
            /// Gets the locality.
            /// </summary>
            public static string Locality
            {
                get
                {
                    return "Locality";
                }
            }

            /// <summary>
            /// Gets the region.
            /// </summary>
            public static string Region
            {
                get
                {
                    return "Region";
                }
            }

            /// <summary>
            /// Gets the postal code.
            /// </summary>
            public static string PostalCode
            {
                get
                {
                    return "PostalCode";
                }
            }

            /// <summary>
            /// Gets the country code.
            /// </summary>
            public static string CountryCode
            {
                get
                {
                    return "CountryCode";
                }
            }
        }

        #endregion

        #region MIME Types


        /// <summary>
        /// Mime type names
        /// </summary>
        public static class MimeTypeNames
        {
            /// <summary>
            /// Application mime types
            /// </summary>
            public static class Application
            {
                /// <summary>
                /// Gets the javascript application mime type
                /// </summary>
                public static string JavaScript
                {
                    get { return "application/x-javascript"; }
                }

                /// <summary>
                /// Gets the JSON application mime type
                /// </summary>
                public static string Json
                {
                    get { return "application/json"; }
                }
            }

            /// <summary>
            /// Text mime types
            /// </summary>
            public static class Text
            {
                /// <summary>
                /// Gets the css mime type
                /// </summary>
                public static string Css
                {
                    get { return "text/css"; }
                }

                /// <summary>
                /// Gets the csv mime type
                /// </summary>
                public static string Csv
                {
                    get { return "text/csv"; }
                }
            }
        }
        #endregion
    }
}