using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;

namespace Entities.Models
{
    /// <summary>
    /// Class <c>Goal</c> contains the goal details.
    /// </summary>
    [Table("goal")]
    public class Goal : BaseModel
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The title of the goal.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The description of the goal.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The start date of the goal.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The end date of the goal.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The status of the goal.
        /// </summary>
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        /// <summary>
        /// The priority of the goal.
        /// </summary>
        [EnumDataType(typeof(Priority))]
        public Priority Priority { get; set; }

        /// <summary>
        /// The progress of the goal.
        /// </summary>
        public decimal Progress { get; set; }
    }
}
