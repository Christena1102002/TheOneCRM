using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.SupportTickets
{
    public class SupportDashboardDto
    {
        // كارت: تم حلها اليوم
        public int ResolvedToday { get; set; }
        public int ResolvedTodayTarget { get; set; } = 10;

        // كارت: التذاكر المفتوحة (+ كام منها أولوية عالية)
        public int OpenTickets { get; set; }
        public int OpenHighPriority { get; set; }

        // كارت: العملاء المعينون وتفاصيلهم
        public int AssignedCustomers { get; set; }
        public int ConsultedCustomers { get; set; }
        public int WaitingConsultation { get; set; }
        public int CustomersWithNotes { get; set; }

        // كارت: تذاكر حرجة (أولوية عالية)
        public int CriticalTickets { get; set; }

        // رسم دائري: التذاكر حسب الحالة
        public List<TicketStatusCountDto> TicketsByStatus { get; set; } = new();

        // رسم أعمدة: التذاكر المحلولة أسبوعياً (آخر 7 أيام)
        public List<WeeklyResolvedDto> WeeklyResolved { get; set; } = new();
    }

    public class TicketStatusCountDto
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public int Count { get; set; }
    }

    public class WeeklyResolvedDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
