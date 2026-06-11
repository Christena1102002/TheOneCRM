using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.DTOs.SupportTickets;

namespace TheOneCRM.Application.Interfaces.ISupportTickets
{
    public interface ISupportTicketService
    {
        Task<SupportTicketResponseDto> CreateTicketAsync(CreateSupportTicketDto dto, string userId);
        Task<SupportTicketResponseDto> GetTicketByIdAsync(int id, string userId, bool isAdmin);
        Task<Pagination<SupportTicketResponseDto>> GetTicketsAsync(SupportTicketParams p, string userId, bool isAdmin);
        Task<SupportTicketResponseDto> UpdateTicketAsync(int id, UpdateSupportTicketDto dto, string userId, bool isAdmin);
        Task DeleteTicketAsync(int id, string userId, bool isAdmin);
        Task<SupportTicketStatisticsDto> GetTicketStatisticsAsync(string userId, bool isAdmin);
        List<StatusClientDto> GetTicketStatuses();
        List<StatusClientDto> GetTicketPriorities();
        Task<List<StatusClientDto>> GetServicesByCustomerAsync(int customerId);
        Task<SupportDashboardDto> GetSupportDashboardAsync(string userId);
        Task<SupportCustomerStatsDto> GetCustomerStatsAsync(string userId, bool isAdmin);
    }
}
