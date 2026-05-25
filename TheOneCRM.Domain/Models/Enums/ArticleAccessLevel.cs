using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Helpers;

namespace TheOneCRM.Domain.Models.Enums
{
    [TypeConverter(typeof(DescriptionEnumConverter))]
    public enum ArticleAccessLevel
    {
        [Description("عام - الجميع")] Public = 0,
        [Description("المطورين فقط")] DevelopersOnly = 1,
        [Description("الدعم الفني فقط")] SupportOnly = 2,
        [Description("الإدارة فقط")] ManagementOnly = 3,
        [Description("السيلز فقط")] SalesOnly = 4
    }
}
