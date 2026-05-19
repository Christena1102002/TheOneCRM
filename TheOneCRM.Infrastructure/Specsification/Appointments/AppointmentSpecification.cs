using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Appointments
{
    public class AppointmentSpecification : BaseSpecification<Appointment>
    {
        public AppointmentSpecification(int id) : base(x => x.Id == id)
        {
        }
        public AppointmentSpecification(AppointmentSpecParams p, string? currentUserId)
       : base(x =>
           (!p.FromDate.HasValue || x.StartDate >= p.FromDate.Value) &&
           (!p.ToDate.HasValue || x.StartDate <= p.ToDate.Value) &&
           //(!p.Type.HasValue || x.Type == p.Type.Value) &&
           //(!p.Status.HasValue || x.Status == p.Status.Value) &&
           (string.IsNullOrEmpty(p.AssignedToUserId) || x.AssignedToId == p.AssignedToUserId) &&
           (!p.CustomerId.HasValue || x.CustomerId == p.CustomerId.Value) &&
           (string.IsNullOrEmpty(p.Search) ||
               x.Title.Contains(p.Search) ||
               (x.Customer.FullName != null && x.Customer.FullName.Contains(p.Search)))
       )
        {
            ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);

        }
    }
}
