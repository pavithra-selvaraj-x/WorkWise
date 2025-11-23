using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Entities.Dtos
{
    /// <summary>
    /// The GoalDetailsDto used to represent the details of a user's goal.
    /// </summary>
    [DataContract]
    public class GoalDetailsDto
    {
        /// <summary>
        /// Gets or sets the goal set by the user.
        /// </summary>
        /// <value>The user's goal.</value>
        [Required(ErrorMessage = "Goal is required.")]
        [DataMember(Name = "goal")]
        public string Goal { get; set; }

        /// <summary>
        /// Gets or sets the importance of the goal as perceived by the user.
        /// </summary>
        /// <value>The importance of the goal.</value>
        [Required(ErrorMessage = "Goal is required.")]
        [DataMember(Name = "importance")]
        public string Importance { get; set; }

        /// <summary>
        /// Gets or sets the timeframe chosen by the user to achieve the goal.
        /// </summary>
        /// <value>The timeframe chosen by the user (in months).</value>
        [Required(ErrorMessage = "Time Frame is required.")]
        [DataMember(Name = "time_frame")]
        public int TimeFrame { get; set; }

        /// <summary>
        /// Gets or sets the amount of time the user can spend per week on the goal.
        /// </summary>
        /// <value>The time per week in hours.</value>
        [Required(ErrorMessage = "Time Per Week is required.")]
        [DataMember(Name = "time_per_week")]
        public int TimePerWeek { get; set; }
    }
}
