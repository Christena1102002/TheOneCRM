using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.Contracts;

namespace TheOneCRM.Application.Interfaces.IContracts
{
    public interface IContractService
    {
        Task<CustomerLookupDto> GetCustomerLookupAsync(int customerId);
        Task CreateContractAsync(CreateContractDto dto, string userId);
        Task<ContractResponseDto> GetContractByIdAsync(int id, string? ownerId);
        Task<Pagination<ContractResponseDto>> GetContractsAsync(ContractParams p, string? ownerId);
        Task UpdateContractAsync(int id, UpdateContractDto dto, string? ownerId);
        Task DeleteContractAsync(int id);
        Task UpdateExpiredContractsAsync();
        Task<ContractStatisticsDto> GetContractStatisticsAsync(string? ownerId);
    }
}
