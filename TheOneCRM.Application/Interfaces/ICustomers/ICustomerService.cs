using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

namespace TheOneCRM.Application.Interfaces.ICustomers
{
    public interface ICustomerService
    {
        Task<CustomerListItemDto> UpdateCustomerAsync(int id, UpdateCustomerDto dto);
        Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto);
        Task<Pagination<CustomerListItemDto>> GetAllCustomersAsync(CustomerPaginationParams paginationParams);
        Task<IReadOnlyList<CustomerListItemDto>> SearchCustomersAsync(string? searchTerm);
        List<StatusClientDto> GetCustomerStatuses();
        Task DeleteCustomerAsync(int id);
        Task<CustomerDetailsDto> GetCustomerByIdAsync(int id);
        Task<CustomerListItemDto> AssignToSalesPersonAsync(int id, string salesPersonId);

        Task<Pagination<CustomerListItemDto>> GetAllgetSalesCustomers(CustomerPaginationParams paginationParams, string? currentUserId, bool isSalesOnly);
        Task UpdateCustomerNoteAsync(int customerId, string note, string userId);
    }
}
