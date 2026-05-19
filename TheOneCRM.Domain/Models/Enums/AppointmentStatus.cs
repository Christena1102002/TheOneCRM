using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Enums
{
    public enum AppointmentStatus
    {
        Scheduled = 1,   // مجدول
        Completed = 2,   // مكتمل
        Cancelled = 3,   // ملغي
        Postponed = 4,   // مؤجل
        NoShow = 5       // لم يحضر
    }
}
