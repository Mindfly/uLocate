//namespace uLocate.IO
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using FileHelpers;

//    [DelimitedRecord(",")]
//    [IgnoreEmptyLines()]
//    [IgnoreFirst(1)] 
//    public class LocationFlat
//    {
//        //[FieldNullValue(typeof(string), "")]
//        //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        //public string LocationTypeName;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string LocationName;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string Address1;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string Address2;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string Locality;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string Region;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string PostalCode;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string CountryCode;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string PhoneNumber;

//        [FieldNullValue(typeof(string), "")]
//        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        public string Email;

//        [FieldNullValue(typeof(double), "0")]
//        public double Longitude;

//        [FieldNullValue(typeof(double), "0")]
//        public double Latitude;

//        //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
//        //public string x;

//    }
//}
