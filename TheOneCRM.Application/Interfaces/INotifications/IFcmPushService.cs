namespace TheOneCRM.Application.Interfaces.INotifications
{
    public interface IFcmPushService
    {
        // يبعت push notification لمستخدم واحد (لو عنده FcmToken)
        Task<bool> SendToUserAsync(string userId, string title, string body, IDictionary<string, string>? data = null);
    }
}
