using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.SupportTickets;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.SupportTicketSpec
{
    public class SupportTicketsListSpec : BaseSpecification<SupportTickets>
    {
        public SupportTicketsListSpec(SupportTicketParams p, string userId, bool isAdmin)
            : base(SupportTicketFilter.Build(p, userId, isAdmin))
        {
            ApplyOrderByDescending(t => t.CreatedAt);
            ApplyPaging((p.PageIndex - 1) * p.PageSize, p.PageSize);
        }
    }

    // فلتر مشترك بين List و Count — بيحوّل الـ Status/Priority (id أو اسم) لقيمة enum
    // عشان المقارنة تترجم لـ SQL صح (EF مش بيترجم enum.ToString())،
    // وبيقصر النتيجة على تذاكر الموظف الحالي إلا لو Admin.
    internal static class SupportTicketFilter
    {
        public static Expression<Func<SupportTickets, bool>> Build(
            SupportTicketParams p, string userId, bool isAdmin)
        {
            StatusOfTickets? status = ParseStatus(p.Status);
            PriorityStatus? priority = ParsePriority(p.Priority);

            return t =>
                (isAdmin || t.CreatedById == userId) &&
                (string.IsNullOrEmpty(p.Search) ||
                    t.Title.Contains(p.Search) ||
                    t.Description.Contains(p.Search) ||
                    t.Customer.FullName.Contains(p.Search) ||
                    t.Service.NameAr.Contains(p.Search)) &&
                (!status.HasValue || t.Status == status.Value) &&
                (!priority.HasValue || t.priority == priority.Value);
        }

        private static StatusOfTickets? ParseStatus(string? value)
            => Enum.TryParse<StatusOfTickets>(value, ignoreCase: true, out var s)
               && Enum.IsDefined(typeof(StatusOfTickets), s)
                ? s
                : (StatusOfTickets?)null;

        private static PriorityStatus? ParsePriority(string? value)
            => Enum.TryParse<PriorityStatus>(value, ignoreCase: true, out var pr)
               && Enum.IsDefined(typeof(PriorityStatus), pr)
                ? pr
                : (PriorityStatus?)null;
    }
}
