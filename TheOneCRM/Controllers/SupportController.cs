using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Application.Interfaces.ISupportTickets;
using TheOneCRM.Domain.Models.Constants;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ISupportTicketService _ticketService;

        public SupportController(
            ICustomerService customerService,
            ISupportTicketService ticketService)
        {
            _customerService = customerService;
            _ticketService = ticketService;
        }

        // GET: api/Support/Dashboard — داشبورد موظف الدعم الحالي
        [HttpGet("Dashboard")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Support}")]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _ticketService.GetSupportDashboardAsync(userId);
            return Ok(new ApiResponse(200, "Support dashboard retrieved successfully", result));
        }

        // POST: api/Support/{id}/ReturnToSalesPerson
        // الدعم يرجّع العميل لمندوب المبيعات اللي حوّله للدعم أصلاً (من الـ AssignmentHistory)
        [HttpPost("{id}/ReturnToSalesPerson")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Support}")]
        public async Task<IActionResult> ReturnToSalesPerson(int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var role = User.GetPrimaryRole();
            if (string.IsNullOrEmpty(role))
                return Forbid();

            var result = await _customerService.ReturnCustomerToSalesAsync(id, userId, role);

            return Ok(new ApiResponse(200, "Customer returned to sales person successfully", result));
        }
    }
}
