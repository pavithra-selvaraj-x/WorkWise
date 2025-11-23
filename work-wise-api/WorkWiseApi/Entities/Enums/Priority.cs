using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Entities.Enums
{
    /// <summary>
    /// Represents different priority levels.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum Priority
    {
        /// <summary>
        /// Enum Low for Low priority
        /// </summary>
        [EnumMember(Value = "Low")]
        Low = 0,
        
        /// <summary>
        /// Enum Medium for Medium priority
        /// </summary>
        [EnumMember(Value = "Medium")]
        Medium = 1,
        
        /// <summary>
        /// Enum High for High priority
        /// </summary>
        [EnumMember(Value = "High")]
        High = 2,
        
        /// <summary>
        /// Enum Urgent for Urgent priority
        /// </summary>
        [EnumMember(Value = "Urgent")]
        Urgent = 3
    }
}
