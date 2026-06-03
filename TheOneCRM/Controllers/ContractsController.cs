using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.Interfaces.IContracts;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.Contracts;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales}")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractsController(IContractService contractService)
        {
            _contractService = contractService;
        }

        // GET: api/contracts/customer-lookup/5
        [HttpGet("customerLookup/{customerId:int}")]
        public async Task<IActionResult> GetCustomerLookup(int customerId)
        {
            var result = await _contractService.GetCustomerLookupAsync(customerId);
            return Ok(new ApiResponse(200, "Customer lookup retrieved successfully", result));
        }
        [HttpPost("CreateContract")]
        public async Task<IActionResult> CreateContract(CreateContractDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _contractService.CreateContractAsync(dto, userId);
            return Ok(new ApiResponse(200, "Contract created successfully"));

        }
        // GET: api/contracts/5
        [HttpGet("GetContractById/{id:int}")]
        public async Task<IActionResult> GetContract(int id)
        {
            // الأدمن يشوف أي عقد (null)، السيلز يشوف اللي هو أنشأه بس
            var ownerId = User.IsAdmin() ? null : User.GetUserId();
            var contract = await _contractService.GetContractByIdAsync(id, ownerId);
            return Ok(new ApiResponse(200, "Contract retrieved successfully", contract));
        }
        [HttpGet("GetContracts")]
        public async Task<IActionResult> GetContracts([FromQuery] ContractParams p)
        {
            // الأدمن يشوف الكل (null)، السيلز يشوف عقوده هو بس
            var ownerId = User.IsAdmin() ? null : User.GetUserId();
            var result = await _contractService.GetContractsAsync(p, ownerId);
            return Ok(new ApiResponse(200, "Contract retrieved successfully", result));
        }
        // PUT: api/contracts/5
        [HttpPut("UpdateContract/{id:int}")]
        public async Task<IActionResult> UpdateContract(int id, UpdateContractDto dto)
        {
            // الأدمن يعدّل أي عقد (null)، السيلز يعدّل عقده هو بس
            var ownerId = User.IsAdmin() ? null : User.GetUserId();
            await _contractService.UpdateContractAsync(id, dto, ownerId);
            return Ok(new ApiResponse(200, "Contract updated successfully"));
        }
        // DELETE: api/contracts/5
        [HttpDelete("DeleteContract/{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteContract(int id)
        {
            await _contractService.DeleteContractAsync(id);
            return Ok(new ApiResponse(200, "Contract deleted successfully" ));
        }
 
        [HttpGet("ContractStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            // الأدمن يشوف إحصائيات الكل، السيلز يشوف إحصائيات عقوده هو بس
            var ownerId = User.IsAdmin() ? null : User.GetUserId();
            var result = await _contractService.GetContractStatisticsAsync(ownerId);

            return Ok(new ApiResponse(200, "Contract statistics retrieved successfully", result));
        }
    }
}
