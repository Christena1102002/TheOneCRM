using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Interfaces.IPriceQuotation;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales}")]
    public class PriceQuotationsController : ControllerBase
    {
        private readonly IPriceQuotationService _priceQuotationService;

        public PriceQuotationsController(IPriceQuotationService priceQuotationService)
        {
            _priceQuotationService = priceQuotationService;
        }

        // الأدمن يشوف الكل (null)، السيلز يشوف عروضه هو بس
        private string? OwnerId() => User.IsAdmin() ? null : User.GetUserId();

        [SwaggerOperation(Summary = "POST: Create new Price Quotation")]
        [HttpPost]
        public async Task<IActionResult> CreatePriceQuotation([FromBody] CreatePriceQuotationDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _priceQuotationService.CreatePriceQuotationAsync(dto, userId);
            return StatusCode(200,
                new ApiResponse(200, "Create new Price Quotation successfully", result));
        }
        [SwaggerOperation(Summary = "GET: List all Price Quotations (paginated + filter)")]
        [HttpGet]
        public async Task<IActionResult> GetAllPriceQuotations([FromQuery] PriceQuotationParams p)
        {
            var result = await _priceQuotationService.GetAllPriceQuotationsAsync(p, OwnerId());
            return StatusCode(200,
                new ApiResponse(200, "تم جلب عروض الأسعار بنجاح", result));
        }
        [SwaggerOperation(Summary = "GET: Get Price Quotation by Id with details")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPriceQuotationById(int id)
        {
            var result = await _priceQuotationService.GetPriceQuotationByIdAsync(id, OwnerId());
            return StatusCode(200,
                new ApiResponse(200, "تم جلب عرض السعر بنجاح", result));
        }
        [SwaggerOperation(Summary = "PUT: Update existing Price Quotation")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePriceQuotation(int id, [FromBody] UpdatePriceQuotationDto dto)
        {
            var result = await _priceQuotationService.UpdatePriceQuotationAsync(id, dto, OwnerId());
            return StatusCode(200,
                new ApiResponse(200, "تم تحديث عرض السعر بنجاح", result));
        }
        [SwaggerOperation(Summary = "DELETE: Delete Price Quotation by Id")]
        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeletePriceQuotation(int id)
        {
            await _priceQuotationService.DeletePriceQuotationAsync(id);
            return StatusCode(200,
                new ApiResponse(200, "تم حذف عرض السعر بنجاح", null));
        }
    }
}
