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
    public enum ArticleStatus
    {
        [Description("مسودة")] Draft = 0,
        [Description("منشورة")] Published = 1
    }
}
