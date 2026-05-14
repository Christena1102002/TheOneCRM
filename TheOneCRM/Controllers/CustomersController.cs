using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.API.Extensions;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
    
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing},{UserRoles.Sales}")]
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var role = User.GetPrimaryRole();
            if (string.IsNullOrEmpty(role))
                return Forbid();
            var result = await _customerService.CreateCustomerAsync(dto, userId, role);

            return StatusCode(201,
                new ApiResponse(201, "Customer created successfully", result));
        }
       
        [SwaggerOperation(Summary = "GET:Customer/dropdown Customers in marketing")]
        [HttpGet("dropdownCustomers")]
        public async Task<IActionResult> GetdropdownCustomers()
        {
            var result = await _customerService.GetCustomersForDropdownAsync();
            return StatusCode(200,
                 new ApiResponse(200, "Get dropdown Customers successfully", result));
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
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Marketing},{UserRoles.Sales}")]
        public async Task<IActionResult> AssignToSalesPerson(int id, [FromBody] AssignCustomerDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var role = User.GetPrimaryRole();
            if (string.IsNullOrEmpty(role))
                return Forbid();

            var result = await _customerService.AssignToSalesPersonAsync(id, dto.SalesPersonId, userId,role);
            return StatusCode(200,
                   new ApiResponse(200, "Assign To SalesPerson successfully"));
        }
        [HttpPost("{id}/AssignToSupportPerson")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Sales},{UserRoles.Support}")]
        public async Task<IActionResult> AssignToSupportPerson(
    int id, [FromBody] AssignToSupportPersonDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var role = User.GetPrimaryRole();
            if (string.IsNullOrEmpty(role))
                return Forbid();

            var result = await _customerService.AssignToSupportPersonAsync(
                id, dto.SupportPersonId, userId, role);

            return Ok(new ApiResponse(200, "Customer assigned to support person successfully", result));
        }
        [HttpPut("{id}/status")]
        //[Authorize] // أو [Authorize(Roles = "Admin,Sales")] حسب صلاحياتك
        public async Task<IActionResult> UpdateCustomerStatus(
    int id,
    [FromBody] UpdateCustomerStatusDto dto)
        {
            var result = await _customerService.UpdateCustomerStatusAsync(id, dto);
            return StatusCode(200,
                  new ApiResponse(200, "Update Customer Status successfully", result));
        }
        [HttpPut("{id}/followUp")]
        //[Authorize] // أو [Authorize(Roles = "Sales,Admin")]
        public async Task<IActionResult> UpdateCustomerFollowUp(
    int id,
    [FromBody] UpdateCustomerFollowUpDto dto)
        {
            var result = await _customerService.UpdateCustomerFollowUpAsync(id, dto);
            return StatusCode(200,
                    new ApiResponse(200, "Update Customer FollowUp successfully", result));
        }
        // GET: api/customers/5
        [HttpGet("{id}/getCustomerById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetCustomerByIdAsync(id);
            return StatusCode(200,
                   new ApiResponse(200, "Get Customer By Id successfully",result));
        }
        [SwaggerOperation(Summary = "all customer already assigned by marketing to sales")]
        [Authorize]
        [HttpGet("getSalesCustomers")]
        public async Task<IActionResult> getSalesCustomers([FromQuery] CustomerPaginationParams paginationParams)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isSalesOnly = User.IsInRole("Sales");
            var result = await _customerService.GetAllgetSalesCustomers(
        paginationParams, userId, isSalesOnly);
            return StatusCode(200,
        new ApiResponse(200, "get customers successfully", result));
        }
        [SwaggerOperation(Summary = "Update customer note by sales")]
        [HttpPatch("customers/{id}/note")]
        public async Task<IActionResult> UpdateCustomerNote(
    int id,
    [FromBody] UpdateCustomerNoteDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _customerService.UpdateCustomerNoteAsync(id, dto.Note, userId);

            return Ok(new ApiResponse(200, "Customer note updated successfully"));
        }
    }
    
}
