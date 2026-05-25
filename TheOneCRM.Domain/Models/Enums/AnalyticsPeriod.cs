using System;
using System.ComponentModel;
using TheOneCRM.Domain.Helpers;

namespace TheOneCRM.Domain.Models.Enums
{
    [TypeConverter(typeof(DescriptionEnumConverter))]
    public enum AnalyticsPeriod
    {
        [Description("الأسبوع الحالي")] CurrentWeek = 0,
        [Description("الشهر الحالي")] CurrentMonth = 1,
        [Description("الربع الحالي")] CurrentQuarter = 2,
        [Description("السنة الحالية")] CurrentYear = 3
    }
}
