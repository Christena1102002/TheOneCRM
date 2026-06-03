using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.NotificationSpec
{
    public class NotificationsByUserSpec : BaseSpecification<Notifications>
    {
        public NotificationsByUserSpec(string userId, bool unreadOnly = false)
            : base(n => n.UserId == userId && (!unreadOnly || !n.IsRead))
        {
            ApplyOrderByDescending(n => n.CreatedAt);
        }
    }
}
