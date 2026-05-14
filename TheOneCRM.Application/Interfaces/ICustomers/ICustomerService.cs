using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

namespace TheOneCRM.Application.Interfaces.ICustomers
{
    public interface ICustomerService
    {
        Task<CustomerListItemDto> UpdateCustomerAsync(int id, UpdateCustomerDto dto);
        Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto, string currentUserId, string currentUserRole);
        Task<Pagination<CustomerListItemDto>> GetAllCustomersAsync(CustomerPaginationParams paginationParams);
        Task<IReadOnlyList<CustomerListItemDto>> SearchCustomersAsync(string? searchTerm);
        List<StatusClientDto> GetCustomerStatuses();
        Task DeleteCustomerAsync(int id);
        Task<CustomerDetailsDto> GetCustomerByIdAsync(int id);
        Task<CustomerListItemDto> AssignToSalesPersonAsync(
    int id, string salesPersonId, string currentUserId, string currentUserRole);

        Task<Pagination<CustomerListItemDto>> GetAllgetSalesCustomers(CustomerPaginationParams paginationParams, string? currentUserId, bool isSalesOnly);
        Task UpdateCustomerNoteAsync(int customerId, string note, string userId);
        Task<IReadOnlyList<CustomerDropdownDto>> GetCustomersForDropdownAsync();
        Task<CustomerListItemDto> AssignToSupportPersonAsync(int id, string SupportPersonId, string currentUserId, string currentUserRole);
        Task<CustomerResponseDto> UpdateCustomerStatusAsync(int id, UpdateCustomerStatusDto dto);
        Task<CustomerResponseDto> UpdateCustomerFollowUpAsync(int id, UpdateCustomerFollowUpDto dto);
        // ✅ Upsert: لو المستخدم عنده ملاحظة على العميل ده، عدّلها. لو لأ، ضيف جديدة
      Task<CustomerNoteResponseDto> UpsertMyNoteAsync(
            int customerId, string note, string userId, string role);
        Task<IReadOnlyList<CustomerNoteResponseDto>> GetNotesByCustomerAsync(
      int customerId, string? role = null);
    }
}
