namespace TheOneCRM.Domain.Models.DTOs.NotificationDtos
{
    public class SendNotificationDto
    {
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
