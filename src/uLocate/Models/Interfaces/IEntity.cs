namespace uLocate.Models
{
    using System;

    /// <summary>
    /// Defines an entity
    /// </summary>
    public interface IEntity
    {
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
        /// Gets the int Id or Guid Key
        /// </summary>
        object IdKey { get; }

        ///// <summary>
        ///// Gets the entity id type (int or guid)
        ///// </summary>
        string EntityIdType { get; }

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