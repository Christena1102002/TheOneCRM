using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Enums
{
    public enum StatusOfTickets
    {
        Open = 0,             // مفتوحة
        InProgress = 1,       // قيد المعالجة
        WaitingCustomer = 2,  // بانتظار العميل
        Resolved = 3,         // تم الحل
        Closed = 4            // مغلقة
    }
}
