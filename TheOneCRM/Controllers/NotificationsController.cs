using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.INotifications;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET /api/Notifications?unreadOnly=false
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] bool unreadOnly = false)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var items = await _notificationService.GetMyNotificationsAsync(userId, unreadOnly);
            return Ok(new ApiResponse(200, "Notifications retrieved successfully", items));
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
    }
}
