using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.Interfaces.ISourceService;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelSourcesController : ControllerBase
    {
        private readonly ISourceService _service;

        public ChannelSourcesController(ISourceService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateChannelSourceDto dto)
        {
            var result = await _service.CreateChannelSourceAsync(dto);
            return StatusCode(200,
                new ApiResponse(200, "create ChannelSource successfully", result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sources = await _service.GetAllChannelSourcesAsync();
            return StatusCode(200,
              new ApiResponse(200, "create ChannelSource successfully", sources));
        }
    }
}
