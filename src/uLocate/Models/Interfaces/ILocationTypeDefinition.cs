namespace uLocate.Models
{
    /// <summary>
    /// Defines a LocationType
    /// </summary>
    public interface ILocationTypeDefinition : IEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the the custom fields associated with the location type.
        /// </summary>
        CustomFieldsCollection Fields { get; } 
    }
}