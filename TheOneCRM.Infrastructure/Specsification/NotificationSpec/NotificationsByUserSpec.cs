using TheOneCRM.Domain.Models.DTOs.NotificationDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.NotificationSpec
{
    public class NotificationsByUserSpec : BaseSpecification<Notifications>
    {
        // للقراءة فقط بدون pagination (للـ MarkAllAsRead أو batch operations)
        public NotificationsByUserSpec(string userId, bool unreadOnly = false)
            : base(n => n.UserId == userId && (!unreadOnly || !n.IsRead))
        {
            ApplyOrderByDescending(n => n.CreatedAt);
        }

        // مع pagination
        public NotificationsByUserSpec(string userId, NotificationParams p)
            : base(n => n.UserId == userId && (!p.UnreadOnly || !n.IsRead))
        {
            ApplyOrderByDescending(n => n.CreatedAt);
            ApplyPaging((p.PageIndex - 1) * p.PageSize, p.PageSize);
        }
    }

    // spec للعدّ فقط (بدون paging)
    public class NotificationsByUserCountSpec : BaseSpecification<Notifications>
    {
        public NotificationsByUserCountSpec(string userId, NotificationParams p)
            : base(n => n.UserId == userId && (!p.UnreadOnly || !n.IsRead))
        {
        }
    }
}
