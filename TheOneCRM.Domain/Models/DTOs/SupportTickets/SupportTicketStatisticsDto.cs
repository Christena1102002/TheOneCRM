using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.SupportTickets
{
    public class SupportTicketStatisticsDto
    {
        public int TotalTickets { get; set; }       // إجمالي التذاكر
        public int ResolvedTickets { get; set; }     // تم الحل
        public int HighPriorityTickets { get; set; } // حرجة (أولوية عالية)
        public int InProgressTickets { get; set; }   // قيد التنفيذ
        public int OpenTickets { get; set; }         // مفتوحة
    }
}
