using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Articles;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

namespace TheOneCRM.Application.Interfaces.IArticles
{
    public interface IArticleService
    {
        // قوائم "الفئة" — المشاريع (للمطوّر/الأدمن) والعملاء (للدعم/المبيعات/الأدمن)
        Task<List<ArticleCategoryOptionDto>> GetProjectOptionsAsync(string userId, bool isAdmin);
        Task<List<ArticleCategoryOptionDto>> GetCustomerOptionsAsync(string userId, bool isAdmin);

        // إنشاء مقالة — نوع الفئة بيتحدد من الرول (المرفقات بترجع كروابط بعد رفعها في الـ controller)
        Task<ArticleResponseDto> CreateArticleAsync(
            CreateArticleDto dto, List<ArticleAttachmentDto> attachments, string userId, IList<string> roles);

        // قائمة المقالات (رؤية حسب الرول) + التفاصيل + التعديل
        Task<Pagination<ArticleResponseDto>> GetArticlesAsync(ArticleParams p, string role, bool isAdmin);
        Task<ArticleResponseDto> GetArticleByIdAsync(int id, string role, bool isAdmin);
        Task<ArticleResponseDto> UpdateArticleAsync(
            int id, UpdateArticleDto dto, string userId, bool isAdmin, IList<string> roles);

        Task DeleteArticleAsync(int id, string userId, bool isAdmin);
        Task DeleteAttachmentAsync(int attachmentId, string userId, bool isAdmin);

        // قوائم بالعربي للـ dropdowns
        List<StatusClientDto> GetTypeOptions();
        List<StatusClientDto> GetAccessLevelOptions();
        List<StatusClientDto> GetStatusOptions();
    }
}
