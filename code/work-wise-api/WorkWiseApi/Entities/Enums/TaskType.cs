using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Entities.Enums
{
    /// <summary>
    /// Represents different types of tasks.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum TaskType
    {
        /// <summary>
        /// Enum Independent for Independent tasks
        /// </summary>
        [EnumMember(Value = "Independent")]
        Independent = 0,

        /// <summary>
        /// Enum GoalRelated for Tasks that are part of a goal
        /// </summary>
        [EnumMember(Value = "GoalRelated")]
        GoalRelated = 1
    }
}
