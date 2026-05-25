using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Application.Helper;
using TheOneCRM.Application.Interfaces.IArticles;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs.Articles;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Infrastructure.Specsification.ArticleSpec;
using TheOneCRM.Infrastructure.Specsification.Customerspec;
using TheOneCRM.Infrastructure.Specsification.ProjectSpec;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Project = TheOneCRM.Domain.Models.Entities.Projects;

namespace TheOneCRM.Application.Services.Articles
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ArticleResponseDto> CreateArticleAsync(
            CreateArticleDto dto, List<ArticleAttachmentDto> attachments, string userId, IList<string> roles)
        {
            // نوع الفئة بيتحدد من الرول، والتحقق إن العنصر موجود
            var categoryType = await ResolveAndValidateCategoryAsync(dto.CategoryId, roles);

            var article = _mapper.Map<Article>(dto);
            article.CreatedById = userId;
            article.CategoryType = categoryType;
            article.CreatedByRole = PrimaryRole(roles);

            if (attachments is not null && attachments.Count > 0)
            {
                article.Attachments = attachments
                    .Select(a => new ArticleAttachment
                    {
                        FileUrl = a.FileUrl,
                        FileName = a.FileName
                    })
                    .ToList();
            }

            await _unitOfWork.Repository<Article>().AddAsync(article);
            await _unitOfWork.SaveChangesAsync();

            return await MapByIdAsync(article.Id);
        }

        public async Task<Pagination<ArticleResponseDto>> GetArticlesAsync(
            ArticleParams p, string role, bool isAdmin)
        {
            var listSpec = new ArticlesListSpec(p, role, isAdmin);
            var countSpec = new ArticlesCountSpec(p, role, isAdmin);

            var items = await _unitOfWork.Repository<Article>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<ArticleResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<Article>().CountAsync(countSpec);

            return new Pagination<ArticleResponseDto>(p.PageIndex, p.PageSize, count, items);
        }

        public async Task<ArticleResponseDto> GetArticleByIdAsync(int id, string role, bool isAdmin)
        {
            var article = await _unitOfWork.Repository<Article>()
                .GetEntityWithSpec(new ArticleByIdSpec(id));

            if (article is null)
                throw new KeyNotFoundException($"Article {id} not found");

            if (!IsVisible(article, role, isAdmin))
                throw new UnauthorizedAccessException("You are not allowed to view this article");

            return _mapper.Map<ArticleResponseDto>(article);
        }

        public async Task<ArticleResponseDto> UpdateArticleAsync(
            int id, UpdateArticleDto dto, string userId, bool isAdmin, IList<string> roles)
        {
            var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id);
            if (article is null)
                throw new KeyNotFoundException($"Article {id} not found");

            // المنشئ أو الأدمن بس
            if (!isAdmin && article.CreatedById != userId)
                throw new UnauthorizedAccessException("This article does not belong to you");

            // الفئة — النوع بيتحدد من الرول زي الإنشاء
            var categoryType = await ResolveAndValidateCategoryAsync(dto.CategoryId, roles);

            _mapper.Map(dto, article);
            article.CategoryType = categoryType;
            article.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Article>().Update(article);
            await _unitOfWork.SaveChangesAsync();

            return await MapByIdAsync(article.Id);
        }

        // جلب المقالة بالـ includes ومابّها للـ response
        private async Task<ArticleResponseDto> MapByIdAsync(int id)
        {
            var article = await _unitOfWork.Repository<Article>()
                .GetEntityWithSpec(new ArticleByIdSpec(id));

            return _mapper.Map<ArticleResponseDto>(article);
        }

        // نفس منطق الرؤية بتاع الـ spec بس in-memory (للـ GetById)
        private static bool IsVisible(Article a, string role, bool isAdmin)
            => isAdmin
               || a.AccessLevel == ArticleAccessLevel.Public
               || a.CreatedByRole == role
               || (a.AccessLevel == ArticleAccessLevel.DevelopersOnly && role == "Developer")
               || (a.AccessLevel == ArticleAccessLevel.SupportOnly && role == "Support")
               || (a.AccessLevel == ArticleAccessLevel.SalesOnly && role == "Sales")
               || (a.AccessLevel == ArticleAccessLevel.ManagementOnly && role == "Admin");

        // الرول الأساسي للمستخدم (للتخزين كـ CreatedByRole)
        private static string PrimaryRole(IList<string> roles)
        {
            if (roles.Contains(UserRoles.Developer)) return UserRoles.Developer;
            if (roles.Contains(UserRoles.Support)) return UserRoles.Support;
            if (roles.Contains(UserRoles.Sales)) return UserRoles.Sales;
            if (roles.Contains(UserRoles.Marketing)) return UserRoles.Marketing;
            if (roles.Contains(UserRoles.Admin)) return UserRoles.Admin;
            return roles.FirstOrDefault() ?? string.Empty;
        }

        // بيحدد نوع الفئة من رول المستخدم ويتأكد إن العنصر (مشروع/عميل) موجود
        private async Task<string> ResolveAndValidateCategoryAsync(int categoryId, IList<string> roles)
        {
            // Developer => مشروع
            if (roles.Contains(UserRoles.Developer))
            {
                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(categoryId);
                if (project is null)
                    throw new KeyNotFoundException($"Project {categoryId} not found");
                return "Project";
            }

            // Support / Sales => عميل
            if (roles.Contains(UserRoles.Support) || roles.Contains(UserRoles.Sales))
            {
                var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(categoryId);
                if (customer is null)
                    throw new KeyNotFoundException($"Customer {categoryId} not found");
                return "Customer";
            }

            // Admin => بيشوف الكل، فنحدد النوع حسب وجود العنصر (مشروع أولاً ثم عميل)
            if (roles.Contains(UserRoles.Admin))
            {
                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(categoryId);
                if (project is not null)
                    return "Project";

                var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(categoryId);
                if (customer is not null)
                    return "Customer";

                throw new KeyNotFoundException($"No project or customer found with id {categoryId}");
            }

            throw new InvalidOperationException("Your role is not allowed to create articles");
        }

        // قوائم بالعربي للـ dropdowns
        public List<StatusClientDto> GetTypeOptions()
            => System.Enum.GetValues<ArticleType>()
                .Select(t => new StatusClientDto { Id = (int)t, Name = ArticleEnumArabic.Type(t) })
                .ToList();

        public List<StatusClientDto> GetAccessLevelOptions()
            => System.Enum.GetValues<ArticleAccessLevel>()
                .Select(a => new StatusClientDto { Id = (int)a, Name = ArticleEnumArabic.AccessLevel(a) })
                .ToList();

        public List<StatusClientDto> GetStatusOptions()
            => System.Enum.GetValues<ArticleStatus>()
                .Select(s => new StatusClientDto { Id = (int)s, Name = ArticleEnumArabic.Status(s) })
                .ToList();

        // المشاريع: المطوّر يشوف مشاريعه، الأدمن يشوف الكل
        public async Task<List<ArticleCategoryOptionDto>> GetProjectOptionsAsync(string userId, bool isAdmin)
        {
            var items = await _unitOfWork.Repository<Project>().ListWithSelectAsync(
                new ProjectsForDropdownSpec(isAdmin ? null : userId),
                p => new ArticleCategoryOptionDto
                {
                    Id = p.Id,
                    Name = p.Title,
                    Type = "Project"
                });

            return items.ToList();
        }

        // العملاء: الدعم/المبيعات يشوفوا عملاءهم، الأدمن يشوف الكل
        public async Task<List<ArticleCategoryOptionDto>> GetCustomerOptionsAsync(string userId, bool isAdmin)
        {
            var items = await _unitOfWork.Repository<Customer>().ListWithSelectAsync(
                new CustomersForDropdownSpec(isAdmin ? null : userId),
                c => new ArticleCategoryOptionDto
                {
                    Id = c.Id,
                    Name = c.FullName,
                    Type = "Customer"
                });

            return items.ToList();
        }
    }
}
