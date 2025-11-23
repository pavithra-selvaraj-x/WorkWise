using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Entities.Enums;

namespace Entities.Dtos
{
    /// <summary>
    /// The TaskDto used to transfer task details.
    /// </summary>
    [DataContract]
    public class TaskDto
    {
        /// <summary>
        /// The unique identifier for the task.
        /// </summary>
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// The unique identifier for the user associated with the task.
        /// </summary>
        [DataMember(Name = "user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// The unique identifier for the goal associated with the task.
        /// </summary>
        [DataMember(Name = "goal_id")]
        public Guid? GoalId { get; set; }

        /// <summary>
        /// The title of the task.
        /// </summary>
        [Required]
        [DataMember(Name = "title")]
        public required string Title { get; set; }

        /// <summary>
        /// The description of the task.
        /// </summary>
        [DataMember(Name = "description")]
        public string? Description { get; set; }

        /// <summary>
        /// The start date of the task.
        /// </summary>
        [DataMember(Name = "start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The end date of the task.
        /// </summary>
        [DataMember(Name = "end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The due date of the task.
        /// </summary>
        [DataMember(Name = "due_date")]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// The status of the task.
        /// </summary>
        [EnumDataType(typeof(Status))]
        [DataMember(Name = "status")]
        public Status Status { get; set; }

        /// <summary>
        /// The priority of the task.
        /// </summary>
        [EnumDataType(typeof(Priority))]
        [DataMember(Name = "priority")]
        public Priority Priority { get; set; }

        /// <summary>
        /// The type of the task.
        /// </summary>
        [EnumDataType(typeof(TaskType))]
        [DataMember(Name = "task_type")]
        public TaskType TaskType { get; set; }
    }
}
