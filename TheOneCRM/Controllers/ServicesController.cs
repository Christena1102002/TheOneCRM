using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.Interfaces.IServices;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.ServicesDtos;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> CreateService(
            [FromBody] CreateServiceDto dto)
        {
            var result = await _servicesService.CreateServiceAsync(dto);
            return StatusCode(200,
            new ApiResponse(200, "added service successfully", result));
        }

        [HttpGet("{id:int}", Name = "GetServiceById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _servicesService.GetServiceByIdAsync(id);
            return StatusCode(200,
            new ApiResponse(200, "Get Service ById successfully", result));
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateService(
    int id, [FromBody] UpdateServiceDto dto)
        {
            var result = await _servicesService.UpdateServiceAsync(id, dto);
            return StatusCode(200,
              new ApiResponse(200, "Update Service successfully", result));
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _servicesService.DeleteServiceAsync(id);
            return StatusCode(200,
             new ApiResponse(200, "Delete Service successfully"));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(
    [FromQuery] ServiceQueryParams queryParams)
        {
            var result = await _servicesService.GetAllServicesAsync(queryParams);
            return StatusCode(200,
             new ApiResponse(200, "Get All Services successfully",result));
        }
    }
}
