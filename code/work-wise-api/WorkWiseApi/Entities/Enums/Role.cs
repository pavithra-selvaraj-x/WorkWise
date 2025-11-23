using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Entities.Enums
{
    /// <summary>
    /// The role of the user.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum Role
    {
        /// <summary>
        /// Enum Admin for Admin user
        /// </summary>
        [EnumMember(Value = "Admin")]
        Admin = 0,
        /// <summary>
        /// Enum UserEnum for Normal User
        /// </summary>
        [EnumMember(Value = "User")]
        User = 1
    }
}
