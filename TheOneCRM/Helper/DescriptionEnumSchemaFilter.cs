using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TheOneCRM.API.Helper
{
    // بيخلي Swagger يعرض الـ enums اللي عليها [Description] بالنص العربي بدل الاسم/الرقم
    public class DescriptionEnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (!type.IsEnum)
                return;

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            // طبّق بس على الـ enums اللي عليها [Description]
            if (!fields.Any(f => f.GetCustomAttribute<DescriptionAttribute>() != null))
                return;

            schema.Enum.Clear();
            schema.Type = "string";
            schema.Format = null;

            foreach (var field in fields)
            {
                var desc = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name;
                schema.Enum.Add(new OpenApiString(desc));
            }
        }
    }
}
