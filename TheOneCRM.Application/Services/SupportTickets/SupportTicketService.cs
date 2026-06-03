using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces.INotifications;
using TheOneCRM.Application.Interfaces.ISupportTickets;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.NotificationDtos;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.DTOs.SupportTickets;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification.Customerspec;
using TheOneCRM.Infrastructure.Specsification.SupportTicketSpec;

namespace TheOneCRM.Application.Services.Tickets
{
    public class SupportTicketService : ISupportTicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public SupportTicketService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<SupportTicketResponseDto> CreateTicketAsync(CreateSupportTicketDto dto, string userId)
        {
            // لو الحالة Open لازم تاريخ المتابعة القادمة
            if (dto.Status == StatusOfTickets.Open && !dto.NextFollowUpDate.HasValue)
                throw new InvalidOperationException(
                    "Next follow-up date is required when the ticket status is Open.");

            // 1) العميل موجود؟
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(dto.CustomerId);
            if (customer is null)
                throw new KeyNotFoundException($"Customer {dto.CustomerId} not found");

            // 2) الخدمة موجودة؟
            var service = await _unitOfWork.Repository<Service>().GetByIdAsync(dto.ServiceId);
            if (service is null)
                throw new KeyNotFoundException($"Service {dto.ServiceId} not found");

            var ticket = _mapper.Map<SupportTickets>(dto);
            ticket.CreatedById = userId;

            // لو اتفتحت Resolved من الأول، سجّل وقت الحل
            if (ticket.Status == StatusOfTickets.Resolved)
                ticket.ResolvedAt = DateTime.UtcNow;

            // 3) توليد رقم التذكرة (آخر رقم + 1، يبدأ من 101)
            var last = await _unitOfWork.Repository<SupportTickets>()
                .GetEntityWithSpec(new LastTicketNumberSpec());
            ticket.TicketNumber = (last?.TicketNumber ?? 100) + 1;

            await _unitOfWork.Repository<SupportTickets>().AddAsync(ticket);
            await _unitOfWork.SaveChangesAsync();

            // إشعار لمندوب المبيعات المخصص للعميل (لو موجود) بوجود تذكرة دعم جديدة
            if (!string.IsNullOrEmpty(customer.AssignedToId))
            {
                await _notificationService.CreateAsync(new CreateNotificationDto
                {
                    UserId = customer.AssignedToId,
                    Title = "تذكرة دعم جديدة",
                    Message = $"تم فتح تذكرة دعم رقم #{ticket.TicketNumber} للعميل '{customer.FullName}'",
                    Type = NotificationType.NewSupportTicket,
                    RelatedEntityType = "SupportTicket",
                    RelatedEntityId = ticket.Id
                });
            }

            return await ProjectTicketAsync(ticket.Id);
        }

        public async Task<SupportTicketResponseDto> GetTicketByIdAsync(int id, string userId, bool isAdmin)
        {
            var ticket = await ProjectTicketAsync(id);

            // موظف الدعم يشوف تذاكره بس، الـ Admin يشوف الكل
            if (!isAdmin && ticket.CreatedById != userId)
                throw new UnauthorizedAccessException("This ticket does not belong to you");

            return ticket;
        }

        public async Task<Pagination<SupportTicketResponseDto>> GetTicketsAsync(
            SupportTicketParams p, string userId, bool isAdmin)
        {
            var listSpec = new SupportTicketsListSpec(p, userId, isAdmin);
            var countSpec = new SupportTicketsCountSpec(p, userId, isAdmin);

            var items = await _unitOfWork.Repository<SupportTickets>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<SupportTicketResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<SupportTickets>().CountAsync(countSpec);

            return new Pagination<SupportTicketResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        public async Task<SupportTicketResponseDto> UpdateTicketAsync(
            int id, UpdateSupportTicketDto dto, string userId, bool isAdmin)
        {
            var ticket = await _unitOfWork.Repository<SupportTickets>().GetByIdAsync(id);
            if (ticket is null)
                throw new KeyNotFoundException($"Ticket {id} not found");

            // موظف الدعم يعدّل تذاكره بس, الـ Admin يعدّل الكل
            if (!isAdmin && ticket.CreatedById != userId)
                throw new UnauthorizedAccessException("This ticket does not belong to you");

            // لو الحالة Open لازم تاريخ المتابعة القادمة
            if (dto.Status == StatusOfTickets.Open && !dto.NextFollowUpDate.HasValue)
                throw new InvalidOperationException(
                    "Next follow-up date is required when the ticket status is Open.");

            // الخدمة موجودة؟
            var service = await _unitOfWork.Repository<Service>().GetByIdAsync(dto.ServiceId);
            if (service is null)
                throw new KeyNotFoundException($"Service {dto.ServiceId} not found");

            var wasResolved = ticket.Status == StatusOfTickets.Resolved;

            _mapper.Map(dto, ticket);
            ticket.UpdatedAt = DateTime.UtcNow;

            // ظبط وقت الحل حسب تغيّر الحالة
            if (ticket.Status == StatusOfTickets.Resolved && !wasResolved)
                ticket.ResolvedAt = DateTime.UtcNow;
            else if (ticket.Status != StatusOfTickets.Resolved)
                ticket.ResolvedAt = null;

            _unitOfWork.Repository<SupportTickets>().Update(ticket);
            await _unitOfWork.SaveChangesAsync();

            return await ProjectTicketAsync(ticket.Id);
        }

        public async Task DeleteTicketAsync(int id, string userId, bool isAdmin)
        {
            var ticket = await _unitOfWork.Repository<SupportTickets>().GetByIdAsync(id);
            if (ticket is null)
                throw new KeyNotFoundException($"Ticket {id} not found");

            // موظف الدعم يمسح تذاكره بس، الـ Admin يمسح الكل
            if (!isAdmin && ticket.CreatedById != userId)
                throw new UnauthorizedAccessException("This ticket does not belong to you");

            _unitOfWork.Repository<SupportTickets>().Delete(ticket);
            await _unitOfWork.SaveChangesAsync();
        }

        // جلب التذكرة كـ DTO (من غير فحص ملكية) — للاستخدام الداخلي بعد create/update
        private async Task<SupportTicketResponseDto> ProjectTicketAsync(int id)
        {
            var ticket = await _unitOfWork.Repository<SupportTickets>()
                .GetQueryableWithSpec(new SupportTicketByIdSpec(id))
                .ProjectTo<SupportTicketResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (ticket is null)
                throw new KeyNotFoundException($"Ticket {id} not found");

            return ticket;
        }

        public async Task<SupportTicketStatisticsDto> GetTicketStatisticsAsync(string userId, bool isAdmin)
        {
            var repo = _unitOfWork.Repository<SupportTickets>();

            // الأدمن: كل التذاكر. السابورت: تذاكره هو بس (CreatedById)
            var tickets = isAdmin
                ? await repo.ListAllAsync()
                : await repo.ListAsync(new SupportTicketsByCreatorSpec(userId));

            return new SupportTicketStatisticsDto
            {
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => t.Status == StatusOfTickets.Open),
                InProgressTickets = tickets.Count(t => t.Status == StatusOfTickets.InProgress),
                ResolvedTickets = tickets.Count(t => t.Status == StatusOfTickets.Resolved),
                HighPriorityTickets = tickets.Count(t => t.priority == PriorityStatus.High)
            };
        }

        // قائمة الحالات (id + اسم عربي) للـ dropdown
        public List<StatusClientDto> GetTicketStatuses()
        {
            return Enum.GetValues<StatusOfTickets>()
                .Select(s => new StatusClientDto
                {
                    Id = (int)s,
                    Name = GetStatusArabicName(s)
                })
                .ToList();
        }

        // قائمة الأولويات (id + اسم عربي) للـ dropdown
        public List<StatusClientDto> GetTicketPriorities()
        {
            return Enum.GetValues<PriorityStatus>()
                .Select(p => new StatusClientDto
                {
                    Id = (int)p,
                    Name = GetPriorityArabicName(p)
                })
                .ToList();
        }

        // داشبورد الدعم — كل المقاييس مقسومة على موظف الدعم الحالي (اللي أنشأ التذاكر)
        public async Task<SupportDashboardDto> GetSupportDashboardAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("User id is required");

            var today = DateTime.UtcNow.Date;
            var weekStart = today.AddDays(-6); // آخر 7 أيام (بما فيها النهاردة)

            // كل تذاكر الموظف
            var tickets = await _unitOfWork.Repository<SupportTickets>()
                .ListAsync(new SupportTicketsByCreatorSpec(userId));

            // العملاء المعينين له
            var assignedCustomers = await _unitOfWork.Repository<Customer>()
                .CountAsync(new AssignedCustomersByUserSpec(userId));

            var dto = new SupportDashboardDto
            {
                ResolvedToday = tickets.Count(t =>
                    t.Status == StatusOfTickets.Resolved &&
                    t.ResolvedAt.HasValue &&
                    t.ResolvedAt.Value.Date == today),

                OpenTickets = tickets.Count(t => t.Status == StatusOfTickets.Open),
                OpenHighPriority = tickets.Count(t =>
                    t.Status == StatusOfTickets.Open && t.priority == PriorityStatus.High),

                CriticalTickets = tickets.Count(t => t.priority == PriorityStatus.High),

                AssignedCustomers = assignedCustomers,

                // التذاكر حسب الحالة (لكل الحالات، حتى لو 0)
                TicketsByStatus = Enum.GetValues<StatusOfTickets>()
                    .Select(s => new TicketStatusCountDto
                    {
                        StatusId = (int)s,
                        StatusName = GetStatusArabicName(s),
                        Count = tickets.Count(t => t.Status == s)
                    })
                    .ToList(),

                // المحلولة أسبوعياً (آخر 7 أيام)
                WeeklyResolved = Enumerable.Range(0, 7)
                    .Select(offset => weekStart.AddDays(offset))
                    .Select(day => new WeeklyResolvedDto
                    {
                        Date = day,
                        Count = tickets.Count(t =>
                            t.Status == StatusOfTickets.Resolved &&
                            t.ResolvedAt.HasValue &&
                            t.ResolvedAt.Value.Date == day)
                    })
                    .ToList()
            };

            return dto;
        }

        private static string GetStatusArabicName(StatusOfTickets status)
        {
            return status switch
            {
                StatusOfTickets.Open => "مفتوحة",
                StatusOfTickets.InProgress => "قيد المعالجة",
                StatusOfTickets.WaitingCustomer => "بانتظار العميل",
                StatusOfTickets.Resolved => "تم الحل",
                StatusOfTickets.Closed => "مغلقة",
                _ => "غير معروف"
            };
        }

        private static string GetPriorityArabicName(PriorityStatus priority)
        {
            return priority switch
            {
                PriorityStatus.High => "عالية",
                PriorityStatus.Medium => "متوسطة",
                PriorityStatus.Low => "منخفضة",
                _ => "غير معروف"
            };
        }
    }
}
