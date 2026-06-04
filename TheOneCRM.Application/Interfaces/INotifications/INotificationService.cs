using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.NotificationDtos;

namespace TheOneCRM.Application.Interfaces.INotifications
{
    public interface INotificationService
    {
        Task CreateAsync(CreateNotificationDto dto);
        Task<Pagination<NotificationResponseDto>> GetMyNotificationsAsync(string userId, NotificationParams p);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int id, string userId);
        Task MarkAllAsReadAsync(string userId);

        // ينشئ إشعارات لكل عميل عنده NextFollowUpDate = النهاردة أو بكرة، للمندوب المخصص
        Task<int> GenerateUpcomingFollowUpRemindersAsync();
    }
}
