namespace uLocate.Models
{
    /// <summary>
    /// Utility extension methods for <see cref="CustomFieldsCollection"/>s.
    /// </summary>
    public static class CustomFieldsCollectionExtensions
    {
        /// <summary>
        /// Utility method to access the Address 1 field
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <returns>
        /// The <see cref="ICustomField"/>.
        /// </returns>
        public static ICustomField Address1(this CustomFieldsCollection fields)
        {
            return fields.GetSafe(Constants.CustomFieldAlias.Address1);
        }

        /// <summary>
        /// Utility method to access the Address 2 field
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <returns>
        /// The <see cref="ICustomField"/>.
        /// </returns>
        public static ICustomField Address2(this CustomFieldsCollection fields)
        {
            return fields.GetSafe(Constants.CustomFieldAlias.Address2);
        }

        /// <summary>
        /// Utility method to access the Locality field
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <returns>
        /// The <see cref="ICustomField"/>.
        /// </returns>
        public static ICustomField Locality(this CustomFieldsCollection fields)
        {
            return fields.GetSafe(Constants.CustomFieldAlias.Locality);
        }

        /// <summary>
        /// Utility method to access the Region field
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <returns>
        /// The <see cref="ICustomField"/>.
        /// </returns>
        public static ICustomField Region(this CustomFieldsCollection fields)
        {
            return fields.GetSafe(Constants.CustomFieldAlias.Region);
        }

        /// <summary>
        /// Utility method to access the PostalCode field
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <returns>
        /// The <see cref="ICustomField"/>.
        /// </returns>
        public static ICustomField PostalCode(this CustomFieldsCollection fields)
        {
            return fields.GetSafe(Constants.CustomFieldAlias.PostalCode);
        }

        /// <summary>
        /// Utility method to access the CountryCode field
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <returns>
        /// The <see cref="ICustomField"/>.
        /// </returns>
        public static ICustomField CountryCode(this CustomFieldsCollection fields)
        {
            return fields.GetSafe(Constants.CustomFieldAlias.CountryCode);
        }

        public static ICustomField GetSafe(this CustomFieldsCollection fields, string alias)
        {
            return fields.ContainsKey(alias) ? fields[alias] : null;
        }
    }
}