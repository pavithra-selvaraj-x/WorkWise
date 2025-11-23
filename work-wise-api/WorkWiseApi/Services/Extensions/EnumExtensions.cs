using System.Reflection;
using System.Runtime.Serialization;

namespace MyExtensions
{
    public static class EnumExtensions
    {
        public static List<string> GetEnumMemberValues<T>() where T : struct, IConvertible
        {
            List<string> list = new List<string>();
            var members = typeof(T)
                 .GetTypeInfo()
                 .DeclaredMembers;
            foreach (var member in members)
            {
                var val = member?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
                if (!string.IsNullOrEmpty(val))
                    list.Add(val);
            }

            return list;
        }

        public static string? GetEnumMemberValue<T>(this T value) where T : Enum
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;
        }
    }
}



