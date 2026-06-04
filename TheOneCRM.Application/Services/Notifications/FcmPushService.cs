using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TheOneCRM.Application.Interfaces.INotifications;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Services.Notify
{
    public class FcmPushService : IFcmPushService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<FcmPushService> _logger;

        public FcmPushService(UserManager<AppUser> userManager, ILogger<FcmPushService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> SendToUserAsync(string userId, string title, string body, IDictionary<string, string>? data = null)
        {
            // Firebase مش متهيأ؟ نتخطى بدون ما نوقع السيرفر
            if (FirebaseApp.DefaultInstance == null)
            {
                _logger.LogWarning("FirebaseApp not initialized. Skipping FCM push for user {UserId}", userId);
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.FcmToken))
                return false;

            var message = new Message
            {
                Token = user.FcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data != null ? new Dictionary<string, string>(data) : null,
                Webpush = new WebpushConfig
                {
                    Notification = new WebpushNotification
                    {
                        Title = title,
                        Body = body,
                        Icon = "/favicon.ico"
                    }
                }
            };

            try
            {
                await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return true;
            }
            catch (FirebaseMessagingException ex)
            {
                _logger.LogWarning(ex, "FCM send failed for user {UserId}", userId);

                // التوكن مش valid → امسحه عشان مانضيعش وقت تاني
                if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                    ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
                {
                    user.FcmToken = null;
                    await _userManager.UpdateAsync(user);
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending FCM push to user {UserId}", userId);
                return false;
            }
        }
    }
}
