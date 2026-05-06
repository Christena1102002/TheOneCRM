using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ICampaignService _campaignService;
        public CustomersController(ICustomerService customerService,ICampaignService campaignService)
        {
            _customerService = customerService;
            _campaignService = campaignService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var result = await _customerService.CreateCustomerAsync(dto);

            return StatusCode(201,
                new ApiResponse(201, "Customer created successfully", result));
        }

        // GET: api/customers/search?term=أحمد
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? SearchPhoneOrName)
        {
            var result = await _customerService.SearchCustomersAsync(SearchPhoneOrName);
            return StatusCode(200,
                 new ApiResponse(200, "get customers successfully", result));
        }
        // GET: api/customers?pageIndex=1&pageSize=10
        [HttpGet("getLeadCustomer")]

        public async Task<IActionResult> GetAll([FromQuery] CustomerPaginationParams paginationParams)
        {
            var result = await _customerService.GetAllCustomersAsync(paginationParams);
            return StatusCode(200,
                  new ApiResponse(200, "get customers successfully", result));
        }
        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses()
        {
            var result = _customerService.GetCustomerStatuses();
            return StatusCode(200,
                 new ApiResponse(200, "get customer statuses successfully", result));
        }
        // PUT: api/customers/5
        [HttpPut("updateCustomer/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            var result = await _customerService.UpdateCustomerAsync(id, dto);
            return StatusCode(200,
                new ApiResponse(200, "update customer successfully", result));
        }
        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return StatusCode(200,
                 new ApiResponse(200, "delete customer successfully"));
        }
        // POST: api/customers/5/assign
        [HttpPost("{id}/assign")]
        public async Task<IActionResult> AssignToSalesPerson(int id, [FromBody] AssignCustomerDto dto)
        {
            var result = await _customerService.AssignToSalesPersonAsync(id, dto.SalesPersonId);
            return StatusCode(200,
                   new ApiResponse(200, "Assign To SalesPerson successfully"));
        }
        // GET: api/customers/5
        [HttpGet("{id}/getCustomerById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            return StatusCode(200,
                   new ApiResponse(200, "Get Customer By Id successfully",result));
        }
    }
}
