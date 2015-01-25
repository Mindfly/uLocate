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
    public static class Constants
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
        /// The allowed standard data types dictionary.
        /// { DataTypeNodeId, "PropertyEditorAlias"}
        /// </summary>
        public static readonly Dictionary<int, string> AllowedStandardDataTypes = new Dictionary<int, string>
        {
            { DataTypeId.TextBox, PropertyEditorAlias.TextBox }, 
            { DataTypeId.TextBoxMultiple, PropertyEditorAlias.TextBoxMultiple }, 
            { DataTypeId.TrueFalse, PropertyEditorAlias.TrueFalse }, 
            { DataTypeId.MultipleMediaPicker, PropertyEditorAlias.MultipleMediaPicker }, 
            { DataTypeId.MemberPicker, PropertyEditorAlias.MemberPicker }, 
            { DataTypeId.ContentPicker, PropertyEditorAlias.ContentPicker }
        };

        /// <summary>
        /// The allowed AllowedPropertyEditors list.
        /// "PropertyEditorAlias"
        /// </summary>
        public static readonly string[] AllowedPropertyEditors = 
        { 
            PropertyEditorAlias.TextBox, 
            PropertyEditorAlias.TextBoxMultiple , 
            PropertyEditorAlias.TrueFalse , 
            PropertyEditorAlias.MultipleMediaPicker , 
            PropertyEditorAlias.MemberPicker , 
            PropertyEditorAlias.ContentPicker, 
            "Umbraco.CheckBoxList", 
            "Umbraco.DropDown",
            "Umbraco.DropDownMultiple"
        };

        /// <summary>
        /// Returns NodeIds for Umbraco Standard DataTypes
        /// </summary>
        public static class DataTypeId
        {
            //TODO: ? Update to verify correct IDs from cmsDataType Table?

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

        #region Database Types

        public const string DbNtext = "Ntext";
        public const string DbNvarchar = "Nvarchar";
        public const string DbInteger = "Integer";
        public const string DbDate = "Date";

        public static class DatabaseTypes
        {
            public static string Ntext
            {
                get
                {
                    return DbNtext;
                }
            }

            public static string Nvarchar
            {
                get
                {
                    return DbNvarchar;
                }
            }

            public static string Integer
            {
                get
                {
                    return DbInteger;
                }
            }

            public static string Date
            {
                get
                {
                    return DbDate;
                }
            }

        }
        #endregion

        #region 'Default' Location Type

        /// <summary>
        /// Gets the "Default" location type key
        /// </summary>
        public static Guid DefaultLocationTypeKey
        {
            get
            {
                return "00EEC1F9-7152-4B3B-B8D5-5B813F86EB66".EncodeAsGuid();
            }
        }

        public static string BaseLocationTypeIcon
        {
            get
            {
                return "icon-pin-location";
            }
        }

        /// <summary>
        /// A list of properties which are part of the 'default' location type.
        /// </summary>
        internal static List<LocationTypeProperty> DefaultLocationTypeProperties
        {
            get
            {
                List<LocationTypeProperty> DefaultProps = new List<LocationTypeProperty>();

                //Address props
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.Address1, Name = "Address 1", DataTypeId = -88, SortOrder = 1,  });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.Address2, Name = "Address 2", DataTypeId = -88, SortOrder = 2 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.Locality, Name = "City", DataTypeId = -88, SortOrder = 3 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.Region, Name = "State/Province", DataTypeId = -88, SortOrder = 4 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.PostalCode, Name = "Zip/Postal Code", DataTypeId = -88, SortOrder = 5 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.CountryCode, Name = "Country", DataTypeId = -88, SortOrder = 6 });
                
                //Other default props
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.Phone, Name = "Phone Number", DataTypeId = -88, SortOrder = 7 });
                DefaultProps.Add(new LocationTypeProperty { LocationTypeKey = Constants.DefaultLocationTypeKey, Alias = DefaultLocPropertyAlias.Email, Name = "Email", DataTypeId = -88, SortOrder = 8 });

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

            /// <summary>
            /// Gets the phone number
            /// </summary>
            public static string Phone
            {
                get
                {
                    return "PhoneNumber";
                }
            }

            /// <summary>
            /// Gets the email address
            /// </summary>
            public static string Email
            {
                get
                {
                    return "Email";
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