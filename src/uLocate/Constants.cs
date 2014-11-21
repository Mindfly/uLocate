namespace uLocate
{
    using System;
    using System.Configuration;

    using Umbraco.Core;

    /// <summary>
    /// uLocate constants
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Gets the Umbraco Package Name
        /// </summary>
        public static string PackageName
        {
            get { return "uLocate"; }
        }

        /// <summary>
        /// The database connection info.
        /// </summary>
        public static class DatabaseConnectionInfo
        {
            /// <summary>
            /// Gets the connection string.
            /// </summary>
            public static string ConnectionString
            {
                get { return ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString; }
            }

            /// <summary>
            /// Gets the provider name.
            /// </summary>
            public static string ProviderName
            {
                get { return ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ProviderName; }
            }
        }


        /// <summary>
        /// Gets the world geodetic coordinate system SRID (spacial reference Id)
        /// </summary>
        public static int WorldGeodeticSystemSrid
        {
            get { return 4326; }
        }

        /// <summary>
        /// Gets the default location type key
        /// </summary>
        public static Guid DefualtLocationTypeKey
        {
            get
            {
                return "00EEC1F9-7152-4B3B-B8D5-5B813F86EB66".EncodeAsGuid();
            }
        }

        /// <summary>
        /// The property editor alias.
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
        }

        /// <summary>
        /// Reserved custom field aliases.
        /// </summary>
        public static class CustomFieldAlias
        {
            /// <summary>
            /// Gets the address 1.
            /// </summary>
            public static string Address1
            {
                get
                {
                    return "ulocateAddress1";
                }
            }

            /// <summary>
            /// Gets the address 2.
            /// </summary>
            public static string Address2
            {
                get
                {
                    return "ulocateAddress2";
                }
            }

            /// <summary>
            /// Gets the locality.
            /// </summary>
            public static string Locality
            {
                get
                {
                    return "ulocateLocality";
                }
            }

            /// <summary>
            /// Gets the region.
            /// </summary>
            public static string Region
            {
                get
                {
                    return "ulocateRegion";
                }
            }

            /// <summary>
            /// Gets the postal code.
            /// </summary>
            public static string PostalCode
            {
                get
                {
                    return "ulocatePostalCode";
                }
            }

            /// <summary>
            /// Gets the country code.
            /// </summary>
            public static string CountryCode
            {
                get
                {
                    return "ulocateCountryCode";
                }
            }
        }

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
                /// Gets the java script application mime type
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
                    get { return "application/json";  }
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
    }
}