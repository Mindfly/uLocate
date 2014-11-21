namespace uLocate.Models
{
    using System;

    /// <summary>
    /// Base class for uLocate entities.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public virtual Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether the entity has an identity.
        /// </summary>
        public virtual bool HasIdentity
        {
            get
            {
                return !Key.Equals(Guid.Empty);
            }
        }

        /// <summary>
        /// Utility method used to update the update date when the entity is about to be updated
        /// </summary>
        public virtual void UpdatingEntity()
        {
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// Utility method used to update the update date when the entity is about to be created
        /// </summary>
        public virtual void AddingEntity()
        {
            Key = Guid.NewGuid();

            UpdateDate = DateTime.Now;

            CreateDate = DateTime.Now;
        }
    }
}