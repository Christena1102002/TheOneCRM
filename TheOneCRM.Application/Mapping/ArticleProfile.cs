using AutoMapper;
using TheOneCRM.Application.Helper;
using TheOneCRM.Domain.Models.DTOs.Articles;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Mapping
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            // Create: DTO -> Entity
            CreateMap<CreateArticleDto, Article>()
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.CreatedByRole, opt => opt.Ignore())
                .ForMember(d => d.CategoryType, opt => opt.Ignore()) // بيتحدد من الرول في الـ service
                .ForMember(d => d.Attachments, opt => opt.Ignore());

            // Update: DTO -> Entity
            CreateMap<UpdateArticleDto, Article>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedById, opt => opt.Ignore())
                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
                .ForMember(d => d.CreatedByRole, opt => opt.Ignore())
                .ForMember(d => d.CategoryType, opt => opt.Ignore()) // بيتحدد من الرول في الـ service
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.Attachments, opt => opt.Ignore());

            // Entity -> Response (الأسماء بالعربي)
            CreateMap<Article, ArticleResponseDto>()
                .ForMember(d => d.TypeName, opt => opt.MapFrom(s => ArticleEnumArabic.Type(s.Type)))
                .ForMember(d => d.AccessLevelName, opt => opt.MapFrom(s => ArticleEnumArabic.AccessLevel(s.AccessLevel)))
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => ArticleEnumArabic.Status(s.Status)))
                .ForMember(d => d.CreatedByName,
                    opt => opt.MapFrom(s => s.CreatedBy != null ? s.CreatedBy.FullName : null));

            CreateMap<ArticleAttachment, ArticleAttachmentDto>();
        }
    }
}
