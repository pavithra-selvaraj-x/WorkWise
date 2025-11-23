using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;

namespace Entities.Models
{
    /// <summary>
    /// Class <c>Task</c> contains the task details.
    /// </summary>
    [Table("task")]
    public class Task : BaseModel
    {
        /// <summary>
        /// The unique identifier for the user associated with the task.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The unique identifier for the goal associated with the task.
        /// </summary>
        public Guid? GoalId { get; set; }

        /// <summary>
        /// The title of the task.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The description of the task.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The start date of the task.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The end date of the task.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The due date of the task.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// The status of the task.
        /// </summary>
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        /// <summary>
        /// The priority of the task.
        /// </summary>
        [EnumDataType(typeof(Priority))]
        public Priority Priority { get; set; }

        /// <summary>
        /// The type of the task.
        /// </summary>
        [EnumDataType(typeof(TaskType))]
        public TaskType TaskType { get; set; }
    }
}
