using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.IAppointment;
using TheOneCRM.Application.Services.AppointmentS;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Appointments;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification.Appointments;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales},{UserRoles.Support}")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        [HttpPost("CreateAppointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _appointmentService.CreateAppointmentAsync(dto, userId);
            return Ok(new ApiResponse(200, "Appointment created successfully", result));
        }
        [HttpPut("UpdateAppointment/{id:int}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var role = User.GetPrimaryRole();
            if (string.IsNullOrEmpty(role))
                return Forbid();
            var result = await _appointmentService.UpdateAppointmentAsync(
                id, dto, userId, role);
            return Ok(new ApiResponse(200, "Appointment updated successfully", result));
        }
        // GET: api/Appointments/GetAppointmentById/{id}
        [HttpGet("GetAppointmentById/{id:int}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _appointmentService.GetAppointmentByIdAsync(id, userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Get Appointment By Id successfully", result));
        }
        //[SwaggerOperation(Summary = "DELETE: حذف موعد بواسطة الـ Id")]
        [HttpDelete("DeleteAppointment/{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            // استخراج بيانات المستخدم الحالي من الـ JWT
            var currentUserId = User.GetUserId();
            var currentUserRole = User.GetPrimaryRole();

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized(new ApiResponse(401, "المستخدم غير مصرح له"));

            // تنفيذ الحذف
            var result = await _appointmentService.DeleteAppointmentAsync(
                id,
                currentUserId,
                currentUserRole ?? string.Empty
            );

            return Ok(new ApiResponse(
                200,
                "تم حذف الموعد بنجاح",
                result
            ));
        }
        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointments([FromQuery] AppointmentSpecParams specParams)
        {
            var currentUserId = User.GetUserId();


            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized(new ApiResponse(401, "المستخدم غير مصرح له"));

            var (items, totalCount) = await _appointmentService.GetAppointmentsAsync(specParams, currentUserId);

            return Ok(new
            {
                statusCode = 200,
                message = "تم جلب المواعيد بنجاح",
                pageIndex = specParams.PageIndex,
                pageSize = specParams.PageSize,
                totalCount = totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)specParams.PageSize),
                data = items
            });
        }

        // GET: api/Appointments/GetStats
        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            var currentUserId = User.GetUserId();
            

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized(new ApiResponse(401, "المستخدم غير مصرح له"));

            var stats = await _appointmentService.GetStatsAsync(currentUserId);

            return Ok(new
            {
                statusCode = 200,
                message = "تم جلب الإحصائيات بنجاح",
                data = stats
            });
        }
    }
 }
