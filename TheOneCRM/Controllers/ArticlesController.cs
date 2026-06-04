using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOneCRM.API.Error;
using TheOneCRM.API.Extensions;
using TheOneCRM.API.Helper;
using TheOneCRM.Application.Interfaces.IArticles;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Articles;

namespace TheOneCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // POST: api/Articles/CreateArticle  (multipart/form-data — بيقبل مرفقات)
        [HttpPost("CreateArticle")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Developer},{UserRoles.Support},{UserRoles.Sales}")]
        public async Task<IActionResult> CreateArticle(
            [FromForm] CreateArticleDto dto,
            [FromForm] List<IFormFile>? attachments)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // ارفع المرفقات (لو فيه) واجمع روابطها
            var attachmentDtos = new List<ArticleAttachmentDto>();
            if (attachments is not null)
            {
                foreach (var file in attachments.Where(f => f is { Length: > 0 }))
                {
                    var url = await DocumentSettings.UploadFileAsync(file, "Articles");
                    attachmentDtos.Add(new ArticleAttachmentDto
                    {
                        FileUrl = url,
                        FileName = file.FileName
                    });
                }
            }

            var result = await _articleService.CreateArticleAsync(dto, attachmentDtos, userId, User.GetAllRoles());
            return Ok(new ApiResponse(200, "Article created successfully", result));
        }

        // GET: api/Articles/GetArticles  (رؤية حسب الرول، الأدمن يشوف الكل)
        [HttpGet("GetArticles")]
        [Authorize]
        public async Task<IActionResult> GetArticles([FromQuery] ArticleParams p)
        {
            var role = User.GetPrimaryRole() ?? string.Empty;
            var result = await _articleService.GetArticlesAsync(p, role, User.IsAdmin());
            return Ok(new ApiResponse(200, "Articles retrieved successfully", result));
        }

        // GET: api/Articles/GetArticleById/5
        [HttpGet("GetArticleById/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var role = User.GetPrimaryRole() ?? string.Empty;
            var result = await _articleService.GetArticleByIdAsync(id, role, User.IsAdmin());
            return Ok(new ApiResponse(200, "Article retrieved successfully", result));
        }

        // PUT: api/Articles/UpdateArticle/5  (المنشئ أو الأدمن)
        [HttpPut("UpdateArticle/{id:int}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Developer},{UserRoles.Support},{UserRoles.Sales}")]
        public async Task<IActionResult> UpdateArticle(int id, [FromBody] UpdateArticleDto dto)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _articleService.UpdateArticleAsync(id, dto, userId, User.IsAdmin(), User.GetAllRoles());
            return Ok(new ApiResponse(200, "Article updated successfully", result));
        }

        // GET: api/Articles/Types  (id + اسم عربي)
        //[HttpGet("Types")]
        //public IActionResult GetTypes()
        //    => Ok(new ApiResponse(200, "Article types retrieved successfully", _articleService.GetTypeOptions()));

        //// GET: api/Articles/AccessLevels  (id + اسم عربي)
        //[HttpGet("AccessLevels")]
        //public IActionResult GetAccessLevels()
        //    => Ok(new ApiResponse(200, "Access levels retrieved successfully", _articleService.GetAccessLevelOptions()));

        //// GET: api/Articles/Statuses  (id + اسم عربي)
        //[HttpGet("Statuses")]
        //public IActionResult GetStatuses()
        //    => Ok(new ApiResponse(200, "Article statuses retrieved successfully", _articleService.GetStatusOptions()));

        // GET: api/Articles/ProjectOptions
        // المشاريع: المطوّر يشوف مشاريعه، الأدمن يشوف الكل
        [HttpGet("ProjectOptions")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Developer},{UserRoles.Support}")]
        public async Task<IActionResult> GetProjectOptions()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _articleService.GetProjectOptionsAsync(userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Project options retrieved successfully", result));
        }

        // GET: api/Articles/CustomerOptions
        // العملاء: الدعم/المبيعات يشوفوا عملاءهم، الأدمن يشوف الكل
        [HttpGet("CustomerOptions")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Support},{UserRoles.Sales}")]
        public async Task<IActionResult> GetCustomerOptions()
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _articleService.GetCustomerOptionsAsync(userId, User.IsAdmin());
            return Ok(new ApiResponse(200, "Customer options retrieved successfully", result));
        }
    }
}
