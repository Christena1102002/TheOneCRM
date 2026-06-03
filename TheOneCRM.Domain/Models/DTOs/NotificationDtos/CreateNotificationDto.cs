using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.NotificationDtos
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public string? RelatedEntityType { get; set; }
        public int? RelatedEntityId { get; set; }
    }
}
