namespace uLocate.Search
{
    using Umbraco.Core;

    public class DefaultFieldNames
    {
        //These need to match the ExamineIndex.config fields

        public const string Key = "Key";

        public const string Name = "Name";

        public const string LocationTypeName = "LocationTypeName";

        public const string Address1 = "Address1";

        public const string Address2 = "Address2";

        public const string Locality = "Locality";
        
        public const string Region = "Region";

        public const string PostalCode = "PostalCode";
        
        public const string CountryCode = "CountryCode";

        public const string Phone = "PhoneNumber";

        public const string Email = "Email";

        public const string CustomPropertyData = "CustomPropertyData";

        public const string AllData = "AllData";
    }

    /// <summary>
    /// Some data indicating how to process a given Location property in search
    /// </summary>
    public class SearchProperty 
    {
        public string PropertyName { get; private set; }
        public double BoostMultiplier { get; private set; }
        public double FuzzyMultiplier { get; private set; }
        public bool Wildcard { get; set; }
        
        public SearchProperty(string propertyName, double boostMultipler = 1.0, double fuzzyMultipler = 1.0, bool wildcard = false)
        {
            this.PropertyName = this.CleanName(propertyName);
            this.BoostMultiplier = boostMultipler;
            this.FuzzyMultiplier = fuzzyMultipler;
            this.Wildcard = wildcard;
        }

        private string CleanName(string name)
        {
            return name.ToSafeAlias();
        }
    }
}