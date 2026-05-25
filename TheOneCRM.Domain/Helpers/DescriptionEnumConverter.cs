using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace TheOneCRM.Domain.Helpers
{
    // TypeConverter بيخلي model binding (الفورم/الكويري) يقبل:
    // النص العربي ([Description]) أو اسم العضو الإنجليزي أو الرقم.
    public class DescriptionEnumConverter : EnumConverter
    {
        public DescriptionEnumConverter(Type type) : base(type) { }

        public override object? ConvertFrom(
            ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                s = s.Trim();

                foreach (var field in EnumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var desc = field.GetCustomAttribute<DescriptionAttribute>()?.Description;

                    if (desc != null && string.Equals(desc, s, StringComparison.OrdinalIgnoreCase))
                        return Enum.Parse(EnumType, field.Name);

                    if (string.Equals(field.Name, s, StringComparison.OrdinalIgnoreCase))
                        return Enum.Parse(EnumType, field.Name);
                }

                if (int.TryParse(s, out var num) && Enum.IsDefined(EnumType, num))
                    return Enum.ToObject(EnumType, num);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
