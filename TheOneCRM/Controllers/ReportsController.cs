using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.Interfaces.IDailyReport;
using TheOneCRM.Domain.Models.DTOs.DailyReports;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IDailyReportService _service;

        public ReportsController(IDailyReportService service)
        {
            _service = service;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("المستخدم غير مصرح له");

        private bool IsAdmin() => User.IsInRole("Admin");

        // POST: api/DailyReports
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDailyReportDto dto)
        {
            var result = await _service.CreateDailyReportAsync(dto, GetUserId());
            return StatusCode(200,
                new ApiResponse(200, "Create Daily Report successfully", result));
        }

        // PUT: api/DailyReports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDailyReportDto dto)
        {
            var result = await _service.UpdateDailyReportAsync(id, dto, GetUserId());
            return StatusCode(200,
                   new ApiResponse(200, "Update Daily Report successfully", result));
        }

        // DELETE: api/DailyReports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteDailyReportAsync(id, GetUserId(), IsAdmin());
            return StatusCode(200,
                new ApiResponse(200, "Delete Daily Report successfully"));
        }

        // GET: api/DailyReports/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetDailyReportByIdAsync(id);
            return StatusCode(200,
                new ApiResponse(200, "Get Daily Report successfully", result));
        }

        // GET: api/DailyReports?PageIndex=1&PageSize=10&UserId=...&FromDate=...&ToDate=...
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DailyReportQueryParams p)
        {
            // غير الـ Admin يشوف تقاريره بس
            //if (!IsAdmin())
            //    p.UserId = GetUserId();

            var result = await _service.GetDailyReportsAsync(p);
            return StatusCode(200,
                 new ApiResponse(200, "Get Daily Reports successfully", result));
        }

        // GET: api/DailyReports/my-reports
        [HttpGet("myreports")]
        public async Task<IActionResult> GetMyReports([FromQuery] DailyReportQueryParams p)
        {
            p.UserId = GetUserId();
            var result = await _service.GetDailyReportsAsync(p);
            return StatusCode(200,
                new ApiResponse(200, "Get My Daily Reports successfully", result));
        }

        // GET: api/DailyReports/by-date?date=2026-05-12
        [HttpGet("byDate")]
        public async Task<IActionResult> GetMyReportByDate([FromQuery] DateTime date)
        {
            var result = await _service.GetMyReportByDateAsync(GetUserId(), date);
            if (result == null)
                return NotFound(new { message = $"لا يوجد تقرير بتاريخ {date:yyyy-MM-dd}" });
            return StatusCode(200,
                new ApiResponse(200, "Get My Daily Report by Date successfully", result));
        }
    }
}
   
