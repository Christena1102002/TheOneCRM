using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.INotifications;
using TheOneCRM.Domain.Models.DTOs.NotificationDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;

        public NotificationsController(INotificationService notificationService, UserManager<AppUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        // GET /api/Notifications?pageIndex=1&pageSize=10&unreadOnly=false
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] NotificationParams p)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _notificationService.GetMyNotificationsAsync(userId, p);
            return Ok(new ApiResponse(200, "Notifications retrieved successfully", result));
        }

        // GET /api/Notifications/unread-count
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new ApiResponse(200, "Unread count retrieved", new { unreadCount = count }));
        }

        // PATCH /api/Notifications/{id}/read
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _notificationService.MarkAsReadAsync(id, userId);
            return Ok(new ApiResponse(200, "Notification marked as read"));
        }

        // PATCH /api/Notifications/read-all
        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new ApiResponse(200, "All notifications marked as read"));
        }

        // POST /api/Notifications/generate-followup-reminders
        // ينشئ إشعارات تذكير لكل عميل عنده متابعة اليوم أو غدًا
        [HttpPost("generate-followup-reminders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenerateFollowUpReminders()
        {
            var created = await _notificationService.GenerateUpcomingFollowUpRemindersAsync();
            return Ok(new ApiResponse(200, "Follow-up reminders generated", new { created }));
        }

        // POST /api/Notifications/register-fcm-token
        // الفرونت يبعت الـ device token عشان يستقبل push notifications
        [HttpPost("register-fcm-token")]
        public async Task<IActionResult> RegisterFcmToken([FromBody] RegisterFcmTokenDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Token))
                return BadRequest(new ApiResponse(400, "Token is required"));

            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            user.FcmToken = dto.Token;
            user.FcmTokenUpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok(new ApiResponse(200, "FCM token registered successfully"));
        }

        // DELETE /api/Notifications/fcm-token
        // اليوزر يلغي تسجيل الـ token (لما يعمل logout أو يقفل المتصفح)
        [HttpDelete("fcm-token")]
        public async Task<IActionResult> UnregisterFcmToken()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new ApiResponse(404, "User not found"));

            user.FcmToken = null;
            user.FcmTokenUpdatedAt = null;
            await _userManager.UpdateAsync(user);

            return Ok(new ApiResponse(200, "FCM token removed successfully"));
        }

        // POST /api/Notifications/send
        // الأدمن يبعت إشعار يدوي لأي يوزر
        [HttpPost("send")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest(new ApiResponse(400, "UserId, Title and Message are required"));

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return NotFound(new ApiResponse(404, $"User with id {dto.UserId} not found"));

            await _notificationService.CreateAsync(new CreateNotificationDto
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Type = NotificationType.AdminMessage
            });

            return Ok(new ApiResponse(200, "Notification sent successfully"));
        }
    }
}
