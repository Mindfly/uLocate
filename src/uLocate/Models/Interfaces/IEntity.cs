namespace uLocate.Models
{
    using System;

    /// <summary>
    /// Defines an entity
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether has identity.
        /// </summary>
        bool HasIdentity { get; }

        /// <summary>
        /// The updating entity.
        /// </summary>
        void UpdatingEntity();

        /// <summary>
        /// The adding entity.
        /// </summary>
        void AddingEntity();
    }
}