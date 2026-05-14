using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class CustomerAssignmentHistory : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        // المسؤول السابق (nullable لأول assignment)
        public string? FromUserId { get; set; }
        public AppUser? FromUser { get; set; }
        public string? FromRole { get; set; }

        // المسؤول الجديد
        public string ToUserId { get; set; }
        public AppUser ToUser { get; set; }
        public string ToRole { get; set; }

        // اللي عمل التحويل
        //public string TransferredById { get; set; }
        //public AppUser TransferredBy { get; set; }

        //public string? Reason { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
