namespace uLocate.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a custom field.
    /// </summary>
    internal class CustomField : ICustomField
    {
        /// <summary>
        /// The pre values.
        /// </summary>
        private IEnumerable<object> _preValues; 

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the property editor alias.
        /// </summary>
        public string PropertyEditorAlias { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public int SortOrder { get; set; }
    }
}