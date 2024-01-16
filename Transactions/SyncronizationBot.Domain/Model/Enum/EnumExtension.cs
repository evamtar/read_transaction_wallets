using System.ComponentModel;
using System.Reflection;


namespace SyncronizationBot.Domain.Model.Enum
{
    public static class EnumExtension
    {
        public static string? GetDescription(this System.Enum value)
        {
            return value == null ? "": value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>()?.Description;
        }
    }
}
