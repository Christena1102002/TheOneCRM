using TheOneCRM.Domain.Models.DTOs.NotificationDtos;

namespace TheOneCRM.Application.Interfaces.INotifications
{
    public interface INotificationService
    {
        Task CreateAsync(CreateNotificationDto dto);
        Task<IReadOnlyList<NotificationResponseDto>> GetMyNotificationsAsync(string userId, bool unreadOnly = false);
        Task<int> GetUnreadCountAsync(string userId);
        Task MarkAsReadAsync(int id, string userId);
        Task MarkAllAsReadAsync(string userId);

        // ينشئ إشعارات لكل عميل عنده NextFollowUpDate = النهاردة أو بكرة، للمندوب المخصص
        Task<int> GenerateUpcomingFollowUpRemindersAsync();
    }
}
