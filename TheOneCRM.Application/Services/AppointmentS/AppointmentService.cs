using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces.IAppointment;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.Appointments;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification;
using TheOneCRM.Infrastructure.Specsification.Appointments;
namespace TheOneCRM.Application.Services.AppointmentS
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public AppointmentService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }   
        public async Task<AppointmentResponseDto> CreateAppointmentAsync(CreateAppointmentDto dto, string createdByUserId)
        {
            // 1. التحقق من التواريخ
            if (dto.EndDate <= dto.StartDate)
                throw new InvalidOperationException("تاريخ النهاية لازم يكون بعد تاريخ البداية");
            var assignUser = await _userManager.FindByIdAsync(dto.AssignedToUserId);
            if(assignUser == null)
                throw new KeyNotFoundException($"الموظف المعين برقم {dto.AssignedToUserId} مش موجود");
            // 3. التحقق من العميل لو متربط بيه
            Customer? customer = null;
            if (dto.CustomerId.HasValue)
            {
                customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(dto.CustomerId.Value);
                if (customer == null)
                    throw new KeyNotFoundException($"العميل برقم {dto.CustomerId} مش موجود");
            }
                // 4. إنشاء الـ entity عن طريق AutoMapper
                var appointment = _mapper.Map<Appointment>(dto);
                appointment.CreatedById = createdByUserId;
                _unitOfWork.Repository<Appointment>().AddAsync(appointment);
                var result = await _unitOfWork.SaveChangesAsync();

                // اربط العلاقات قبل الـ map للـ response (في الذاكرة بس)
                appointment.AssignedTo = assignUser;
                appointment.Customer = customer;


                return _mapper.Map<AppointmentResponseDto>(appointment);
            }

        public async Task<AppointmentResponseDto> UpdateAppointmentAsync(int id, UpdateAppointmentDto dto, string currentUserId, string currentUserRole)
        {
            var spec = new AppointmentSpecification(id);
            var appointment = await _unitOfWork.Repository<Appointment>().GetEntityWithSpec(spec);
            if (appointment == null)
                throw new KeyNotFoundException($"الموعد برقم {id} مش موجود");

            // 3. تحقق التواريخ
            if (dto.EndDate <= dto.StartDate)
                throw new InvalidOperationException("تاريخ النهاية لازم يكون بعد تاريخ البداية");

            // 4. تحقق وجود الموظف
            var userExists = await _userManager.Users.AnyAsync(u => u.Id == dto.AssignedToUserId);
            if (!userExists)
                throw new KeyNotFoundException($"الموظف المعين برقم {dto.AssignedToUserId} مش موجود");

            // 5. تحقق وجود العميل لو متربط
            if (dto.CustomerId.HasValue)
            {
                var customerExists = await _unitOfWork.Repository<Customer>()
                    .AnyAsync(c => c.Id == dto.CustomerId.Value);
                if (!customerExists)
                    throw new KeyNotFoundException($"العميل برقم {dto.CustomerId} مش موجود");
            }

            // 6. تطبيق التعديلات
            _mapper.Map(dto, appointment);

            _unitOfWork.Repository<Appointment>().Update(appointment);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new InvalidOperationException("فشل في تحديث الموعد");

            // 7. ⭐ استخدم ApplySpecification + ProjectTo مباشرة
            var response = await _unitOfWork.Repository<Appointment>()
                .ApplySpecification(new AppointmentSpecification(appointment.Id))
                .ProjectTo<AppointmentResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return response!;

        }
        public async Task<AppointmentResponseDto> GetAppointmentByIdAsync(int id, string userId, bool isAdmin)
        {
            var spec = new AppointmentSpecification(id);

            var appointment = await _unitOfWork.Repository<Appointment>()
                .ApplySpecification(spec)
                .ProjectTo<AppointmentResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (appointment == null)
                throw new KeyNotFoundException($"الموعد برقم {id} مش موجود");

            // الأدمن يشوف أي موعد، غيره: لو هو اللي أنشأه أو الموعد معه (AssignedToId)
            if (!isAdmin && appointment.AssignedToId != userId && appointment.CreatedById != userId)
                throw new UnauthorizedAccessException("This appointment does not belong to you");

            return appointment;
        }

        public async Task<bool> DeleteAppointmentAsync(int id, string currentUserId, string currentUserRole)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);

            if (appointment == null)
                throw new KeyNotFoundException($"الموعد برقم {id} مش موجود");

            _unitOfWork.Repository<Appointment>().Delete(appointment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<(IReadOnlyList<AppointmentResponseDto> Items, int TotalCount)> GetAppointmentsAsync(
    AppointmentSpecParams specParams,
    string currentUserId)
        {
            var spec = new AppointmentSpecification(specParams, currentUserId);
            var countSpec = new AppointmentCountSpecification(specParams, currentUserId);

            // Projection للحصول على DTO مباشرة من الـ DB
            var items = await _unitOfWork.Repository<Appointment>()
                .ApplySpecification(spec)
                .ProjectTo<AppointmentResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var totalCount = await _unitOfWork.Repository<Appointment>().CountAsync(countSpec);

            return (items, totalCount);
        }

        // ============ GET STATS ============
        public async Task<AppointmentStatsDto> GetStatsAsync(string currentUserId)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            var nowUtc = DateTime.UtcNow;

            // base query
            var query = _unitOfWork.Repository<Appointment>()
                .ApplySpecification(new AppointmentSpecification(
                    new AppointmentSpecParams { PageSize = int.MaxValue },
                    currentUserId));

            return new AppointmentStatsDto
            {
                Total = await query.CountAsync(),
                Upcoming = await query.CountAsync(x =>
                                x.Status == AppointmentStatus.Scheduled &&
                                x.StartDate >= nowUtc),
                Confirmed = await query.CountAsync(x => x.Status == AppointmentStatus.Completed),
                Today = await query.CountAsync(x =>
                                x.StartDate >= today &&
                                x.StartDate < tomorrow)
            };
        }
    }
    
}
