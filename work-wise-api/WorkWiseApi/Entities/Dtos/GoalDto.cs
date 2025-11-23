using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Entities.Enums;

namespace Entities.Dtos
{
    /// <summary>
    /// The GoalDto used to transfer goal details.
    /// </summary>
    [DataContract]
    public class GoalDto
    {
        /// <summary>
        /// The unique identifier for the goal.
        /// </summary>
        [DataMember(Name = "id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// The title of the goal.
        /// </summary>
        [Required]
        [DataMember(Name = "title")]
        public required string Title { get; set; }

        /// <summary>
        /// The description of the goal.
        /// </summary>
        [DataMember(Name = "description")]
        public string? Description { get; set; }

        /// <summary>
        /// The start date of the goal.
        /// </summary>
        [DataMember(Name = "start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The end date of the goal.
        /// </summary>
        [DataMember(Name = "end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// The status of the goal.
        /// </summary>
        [EnumDataType(typeof(Status))]
        [DataMember(Name = "status")]
        public Status Status { get; set; }

        /// <summary>
        /// The priority of the goal.
        /// </summary>
        [EnumDataType(typeof(Priority))]
        [DataMember(Name = "priority")]
        public Priority Priority { get; set; }

        /// <summary>
        /// The progress of the goal.
        /// </summary>
        [DataMember(Name = "progress")]
        public decimal Progress { get; set; }

        /// <summary>
        /// The list of tasks for the goal.
        /// </summary>
        [DataMember(Name = "task_list")]
        public List<TaskDto>? TaskList { get; set; }
    }
}
