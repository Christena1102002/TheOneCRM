using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.ISupportTickets;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.SupportTickets;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Support}")]
    public class SupportTicketsController : ControllerBase
    {
        private readonly ISupportTicketService _ticketService;

        public SupportTicketsController(ISupportTicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // POST: api/SupportTickets/CreateTicket
        [HttpPost("CreateTicket")]
        public async Task<IActionResult> CreateTicket(CreateSupportTicketDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _ticketService.CreateTicketAsync(dto, userId);
            return Ok(new ApiResponse(200, "Ticket created successfully", result));
        }

        // GET: api/SupportTickets/GetTicketById/5
        [HttpGet("GetTicketById/{id:int}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _ticketService.GetTicketByIdAsync(id, userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Ticket retrieved successfully", result));
        }

        // GET: api/SupportTickets/GetTickets
        [HttpGet("GetTickets")]
        public async Task<IActionResult> GetTickets([FromQuery] SupportTicketParams p)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _ticketService.GetTicketsAsync(p, userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Tickets retrieved successfully", result));
        }

        // PUT: api/SupportTickets/UpdateTicket/5
        [HttpPut("UpdateTicket/{id:int}")]
        public async Task<IActionResult> UpdateTicket(int id, UpdateSupportTicketDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _ticketService.UpdateTicketAsync(id, dto, userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Ticket updated successfully", result));
        }

        // DELETE: api/SupportTickets/DeleteTicket/5
        [HttpDelete("DeleteTicket/{id:int}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _ticketService.DeleteTicketAsync(id, userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Ticket deleted successfully"));
        }

        // GET: api/SupportTickets/TicketStatistics
        [HttpGet("TicketStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _ticketService.GetTicketStatisticsAsync(userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Ticket statistics retrieved successfully", result));
        }

        // GET: api/SupportTickets/TicketStatuses  (للـ dropdown: id + اسم)
        [HttpGet("TicketStatuses")]
        public IActionResult GetTicketStatuses()
        {
            var result = _ticketService.GetTicketStatuses();
            return Ok(new ApiResponse(200, "Ticket statuses retrieved successfully", result));
        }

        // GET: api/SupportTickets/TicketPriorities  (للـ dropdown: id + اسم)
        [HttpGet("TicketPriorities")]
        public IActionResult GetTicketPriorities()
        {
            var result = _ticketService.GetTicketPriorities();
            return Ok(new ApiResponse(200, "Ticket priorities retrieved successfully", result));
        }
    }
}
