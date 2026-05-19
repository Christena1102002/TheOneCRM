using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.Appointments
{
    public class AppointmentStatsDto
    {
        public int Total { get; set; }       // الإجمالي
        public int Upcoming { get; set; }    // قادمة (مجدولة في المستقبل)
        public int Confirmed { get; set; }   // مؤكدة (Completed)
        public int Today { get; set; }       // اليوم
    }
}
