using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos;

namespace TheOneCRM.Application.Interfaces.IPriceQuotation
{
    public interface IPriceQuotationService
    {
        Task<PriceQuotationResponseDto> CreatePriceQuotationAsync(CreatePriceQuotationDto dto, string userId);
        Task<PaginatedPriceQuotationsDto> GetAllPriceQuotationsAsync(PriceQuotationParams p, string? ownerId);
        Task<PriceQuotationResponseDto> GetPriceQuotationByIdAsync(int id, string? ownerId);
        Task<PriceQuotationResponseDto> UpdatePriceQuotationAsync(int id, UpdatePriceQuotationDto dto, string? ownerId);
        Task DeletePriceQuotationAsync(int id);
    }
}
