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
        Task<PriceQuotationResponseDto> CreatePriceQuotationAsync(CreatePriceQuotationDto dto);
        Task<PaginatedPriceQuotationsDto> GetAllPriceQuotationsAsync(PriceQuotationParams p);
        Task<PriceQuotationResponseDto> GetPriceQuotationByIdAsync(int id);
        Task<PriceQuotationResponseDto> UpdatePriceQuotationAsync(int id, UpdatePriceQuotationDto dto);
        Task DeletePriceQuotationAsync(int id);
    }
}
