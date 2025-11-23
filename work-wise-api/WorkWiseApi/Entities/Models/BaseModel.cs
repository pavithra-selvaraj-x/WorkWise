using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    /// <summary>
    /// The base model with the common properties for all other models.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Created date of the entity.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Updated date of the entity.
        /// </summary>
        public DateTime DateUpdated { get; set; }

        /// <summary>
        /// Created by user Id of the entity.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Updated by user Id of the entity.
        /// </summary>
        public Guid UpdatedBy { get; set; }

        /// <summary>
        /// Boolean to indicate whether entity is active or deleted.
        /// </summary>
        public bool IsActive { get; set; }
    }
}