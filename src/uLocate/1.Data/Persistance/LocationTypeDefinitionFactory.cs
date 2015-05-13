namespace uLocate.Persistance
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
        public ILocationType GetEmptyDefaultLocationTypeDefinition()
        {
            var def = new LocationType()
                       {
                           Id = Constants.DefaultLocationTypeId,
                           Name = "Default",
                           Fields = new CustomFieldsCollection()
                       };

            //TODO: HLF - this needs to be fixed...
            def.Fields.SetValue(new CustomField() { Label = "Address 1", Alias = Constants.DefaultLocPropertyAlias.Address1, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 1 });
            def.Fields.SetValue(new CustomField() { Label = "Address 2", Alias = Constants.DefaultLocPropertyAlias.Address2, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 2 });
            def.Fields.SetValue(new CustomField() { Label = "Locality", Alias = Constants.DefaultLocPropertyAlias.Locality, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 3 });
            def.Fields.SetValue(new CustomField() { Label = "Region", Alias = Constants.DefaultLocPropertyAlias.Region, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 4 });
            def.Fields.SetValue(new CustomField() { Label = "Country", Alias = Constants.DefaultLocPropertyAlias.CountryCode, PropertyEditorAlias = Constants.PropertyEditorAlias.DropDownList, SortOrder = 5 });
            def.Fields.SetValue(new CustomField() { Label = "Region", Alias = Constants.DefaultLocPropertyAlias.PostalCode, PropertyEditorAlias = Constants.PropertyEditorAlias.TextBox, SortOrder = 6 });

            return def;
        }
    }
}