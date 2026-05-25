using System;
using System.ComponentModel;
using System.Reflection;

namespace TheOneCRM.Domain.Helpers
{
    public static class EnumExtensions
    {
        // بترجّع نص الـ [Description] لو موجود، وإلا اسم العضو
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}
