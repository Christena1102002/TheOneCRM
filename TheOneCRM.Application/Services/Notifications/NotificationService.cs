using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheOneCRM.Application.Interfaces.INotifications;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.NotificationDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification.NotificationSpec;

namespace TheOneCRM.Application.Services.Notify
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IServiceScopeFactory scopeFactory,
            ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task CreateAsync(CreateNotificationDto dto)
        {
            var entity = new Notifications
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                RelatedEntityType = dto.RelatedEntityType,
                RelatedEntityId = dto.RelatedEntityId,
                IsRead = false
            };

            await _unitOfWork.Repository<Notifications>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            // ابعت Push notification في الـ background (مايأخرش الـ response)
            var data = new Dictionary<string, string>
            {
                ["notificationId"] = entity.Id.ToString(),
                ["type"] = dto.Type.ToString(),
                ["relatedEntityType"] = dto.RelatedEntityType ?? string.Empty,
                ["relatedEntityId"] = dto.RelatedEntityId?.ToString() ?? string.Empty
            };

            // fire-and-forget: scope جديد عشان مايتأثرش بانتهاء الـ request
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fcm = scope.ServiceProvider.GetRequiredService<IFcmPushService>();
                    await fcm.SendToUserAsync(dto.UserId, dto.Title, dto.Message, data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background FCM push failed for user {UserId}", dto.UserId);
                }
            });
        }

        public async Task<Pagination<NotificationResponseDto>> GetMyNotificationsAsync(string userId, NotificationParams p)
        {
            var listSpec = new NotificationsByUserSpec(userId, p);
            var countSpec = new NotificationsByUserCountSpec(userId, p);

            var items = await _unitOfWork.Repository<Notifications>().ListAsync(listSpec);
            var totalCount = await _unitOfWork.Repository<Notifications>().CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<NotificationResponseDto>>(items);

            return new Pagination<NotificationResponseDto>(p.PageIndex, p.PageSize, totalCount, data);
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            var spec = new NotificationsByUserSpec(userId, unreadOnly: true);
            return await _unitOfWork.Repository<Notifications>().CountAsync(spec);
        }

        public async Task MarkAsReadAsync(int id, string userId)
        {
            var notif = await _unitOfWork.Repository<Notifications>().GetByIdAsync(id);
            if (notif == null || notif.UserId != userId) return;

            if (!notif.IsRead)
            {
                notif.IsRead = true;
                notif.ReadAt = DateTime.UtcNow;
                _unitOfWork.Repository<Notifications>().Update(notif);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var spec = new NotificationsByUserSpec(userId, unreadOnly: true);
            var items = await _unitOfWork.Repository<Notifications>().ListAsync(spec);
            if (items.Count == 0) return;

            foreach (var n in items)
            {
                n.IsRead = true;
                n.ReadAt = DateTime.UtcNow;
                _unitOfWork.Repository<Notifications>().Update(n);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> GenerateUpcomingFollowUpRemindersAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var customers = await _unitOfWork.Repository<Customer>().ListAllAsync();

            var due = customers
                .Where(c => c.NextFollowUpDate.HasValue
                            && !string.IsNullOrEmpty(c.AssignedToId)
                            && c.NextFollowUpDate.Value.Date >= today
                            && c.NextFollowUpDate.Value.Date <= tomorrow)
                .ToList();

            if (due.Count == 0) return 0;

            // امنع التكرار: لو فيه إشعار من نفس النوع لنفس العميل اتعمل خلال آخر 24 ساعة، اتخطى
            var since = DateTime.UtcNow.AddHours(-24);
            var allNotifs = await _unitOfWork.Repository<Notifications>().ListAllAsync();
            var alreadyNotified = allNotifs
                .Where(n => n.Type == NotificationType.UpcomingFollowUp
                            && n.RelatedEntityType == "Customer"
                            && n.CreatedAt >= since)
                .Select(n => n.RelatedEntityId)
                .ToHashSet();

            var created = 0;
            foreach (var c in due)
            {
                if (alreadyNotified.Contains(c.Id)) continue;

                var when = c.NextFollowUpDate!.Value.Date == today ? "اليوم" : "غدًا";
                await CreateAsync(new CreateNotificationDto
                {
                    UserId = c.AssignedToId!,
                    Title = "تذكير بمتابعة",
                    Message = $"لديك متابعة {when} مع العميل '{c.FullName}'",
                    Type = NotificationType.UpcomingFollowUp,
                    RelatedEntityType = "Customer",
                    RelatedEntityId = c.Id
                });
                created++;
            }

            return created;
        }
    }
}
