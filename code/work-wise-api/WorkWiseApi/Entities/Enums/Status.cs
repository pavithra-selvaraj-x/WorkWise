using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Entities.Enums
{
    /// <summary>
    /// The status of a task/goal.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum Status
    {
        /// <summary>
        /// Enum Open for Not Started task/goal
        /// </summary>
        [EnumMember(Value = "Open")]
        Open = 0,
        
        /// <summary>
        /// Enum InProgress for In Progress task/goal
        /// </summary>
        [EnumMember(Value = "In Progress")]
        InProgress = 1,
        
        /// <summary>
        /// Enum Completed for Completed task/goal
        /// </summary>
        [EnumMember(Value = "Completed")]
        Completed = 2,
        
        /// <summary>
        /// Enum OnHold for task/goal on hold
        /// </summary>
        [EnumMember(Value = "On Hold")]
        OnHold = 3,
        
        /// <summary>
        /// Enum Cancelled for Cancelled task/goal
        /// </summary>
        [EnumMember(Value = "Cancelled")]
        Cancelled = 4
    }
}
