using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Infrastructure.Specsification.Appointments
{
    public class AppointmentSpecParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        //public AppointmentType? Type { get; set; }
        //public AppointmentStatus? Status { get; set; }
        public string? AssignedToUserId { get; set; }

        /// <summary>عرض مواعيدي فقط</summary>
        //public bool OnlyMine { get; set; } = false;

        public int? CustomerId { get; set; }

        /// <summary>بحث بعنوان أو وصف الموعد</summary>
        public string? Search { get; set; }

        public int PageIndex { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }

    }
}
