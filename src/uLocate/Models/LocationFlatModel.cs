using FileHelpers;

namespace uLocate.IO
{
    [IgnoreEmptyLines]
    [DelimitedRecord(",")]
    [IgnoreFirst(1)]
    public class LocationFlat
    {
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        [FieldNullValue(typeof(string), "")]
        public string LocationName;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        [FieldNullValue(typeof(string), "")]
        public string Address1;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        [FieldNullValue(typeof(string), "")]
        public string Address2;
        [FieldNullValue(typeof(string), "")]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public string Locality;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        [FieldNullValue(typeof(string), "")]
        public string Region;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        [FieldNullValue(typeof(string), "")]
        public string PostalCode;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        [FieldNullValue(typeof(string), "")]
        public string CountryCode;
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        [FieldNullValue(typeof(string), "")]
        public string PhoneNumber;
        [FieldNullValue(typeof(string), "")]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public string Email;
        [FieldNullValue(typeof(string), "0")]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public string Longitude;
        [FieldNullValue(typeof(string), "0")]
        [FieldQuoted('"', QuoteMode.AlwaysQuoted)]
        public string Latitude;

        public string GetProperty(string alias)
        {
            switch (alias)
            {
                case "Address2":
                    return this.Address2;
                case "PhoneNumber":
                    return this.PhoneNumber;
                case "Email":
                    return this.Email;
                case "PostalCode":
                    return this.PostalCode;
                case "CountryCode":
                    return this.CountryCode;
                case "Address1":
                    return this.Address1;
                case "Locality":
                    return this.Locality;
                case "Region":
                    return this.Region;
                default:
                    return "";
            }
        }

        public void SetProperty(string alias, object data)
        {
            switch (alias)
            {
                case "Address2":
                    this.Address2 = data.ToString();
                    break;
                case "PhoneNumber":
                    this.PhoneNumber = data.ToString();
                    break;
                case "Email":
                    this.Email = data.ToString();
                    break;
                case "PostalCode":
                    this.PostalCode = data.ToString();
                    break;
                case "CountryCode":
                    this.CountryCode = data.ToString();
                    break;
                case "Address1":
                    this.Address1 = data.ToString();
                    break;
                case "Locality":
                    this.Locality = data.ToString();
                    break;
                case "Region":
                    this.Region = data.ToString();
                    break;
                case "Latitude":
                    this.Latitude = data.ToString();
                    break;
                case "Longitude":
                    this.Longitude = data.ToString();
                    break;
                case "LocationName":
                    this.LocationName = data.ToString();
                    break;
            }
        }
    }
}