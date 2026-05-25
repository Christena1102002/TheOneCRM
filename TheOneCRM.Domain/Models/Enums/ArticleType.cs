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
    public enum ArticleType
    {
        [Description("دليل")] Guide = 0,
        [Description("إجراء")] Procedure = 1,
        [Description("حل مشكلة")] ProblemSolving = 2,
        [Description("أفضل الممارسات")] BestPractices = 3,
        [Description("مرجع تقني")] TechnicalReference = 4
    }
}
