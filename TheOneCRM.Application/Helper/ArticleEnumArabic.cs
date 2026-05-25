using TheOneCRM.Domain.Helpers;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Application.Helper
{
    // الأسماء العربية لـ enums المقالة — المصدر الوحيد هو [Description] على أعضاء الـ enum
    public static class ArticleEnumArabic
    {
        public static string Type(ArticleType type) => type.GetDescription();
        public static string AccessLevel(ArticleAccessLevel level) => level.GetDescription();
        public static string Status(ArticleStatus status) => status.GetDescription();
    }
}
