using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.API.Extensions;
namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerNotesController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerNotesController(ICustomerService service)
        {
            _customerService = service;
        }
        [HttpPut("customer/{customerId}/myNote")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing},{UserRoles.Sales},{UserRoles.Support}")]
        public async Task<IActionResult> UpsertMyNote(
          int customerId, [FromBody] UpsertCustomerNoteDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var role = User.GetPrimaryRole();
            if (string.IsNullOrEmpty(role))
                return Forbid();

            var result = await _customerService.UpsertMyNoteAsync(customerId, dto.Note, userId, role);

            return Ok(new ApiResponse(200, "Note saved successfully", result));
        }
        // ✅ جلب كل ملاحظات عميل (اختيارياً فلترة بنوع الـ role)
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(
            int customerId, [FromQuery] string? role = null)
        {
            var result = await _customerService.GetNotesByCustomerAsync(customerId, role);
            return Ok(new ApiResponse(200, "Notes retrieved successfully", result));
        }
    }
}
