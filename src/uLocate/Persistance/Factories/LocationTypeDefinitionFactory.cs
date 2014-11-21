namespace uLocate.Persistance.Factories
{
    using uLocate.Models;

    using Constants = uLocate.Constants;

    /// <summary>
    /// The location type definition factory.
    /// </summary>
    internal class LocationTypeDefinitionFactory
    {
        /// <summary>
        /// The get empty default location type definition.
        /// </summary>
        /// <returns>
        /// The <see cref="ILocationTypeDefinition"/>.
        /// </returns>
        public ILocationTypeDefinition GetEmptyDefaultLocationTypeDefinition()
        {
            var def = new LocationTypeDefinition()
                       {
                           Key = Constants.DefualtLocationTypeKey,
                           Name = "Default",
                           Fields = new CustomFieldsCollection()
                       };

            def.Fields.SetValue(new CustomField() { Label = "Address 1", Alias = Constants.CustomFieldAlias.Address1, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 1 });
            def.Fields.SetValue(new CustomField() { Label = "Address 2", Alias = Constants.CustomFieldAlias.Address2, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 2 });
            def.Fields.SetValue(new CustomField() { Label = "Locality", Alias = Constants.CustomFieldAlias.Locality, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 3 });
            def.Fields.SetValue(new CustomField() { Label = "Region", Alias = Constants.CustomFieldAlias.Region, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 4 });
            def.Fields.SetValue(new CustomField() { Label = "Country", Alias = Constants.CustomFieldAlias.CountryCode, PropertyEditorAlias = Constants.PropertyEditorAlias.DropDownList, SortOrder = 5 });
            def.Fields.SetValue(new CustomField() { Label = "Region", Alias = Constants.CustomFieldAlias.PostalCode, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 6 });

            return def;
        }
    }
}