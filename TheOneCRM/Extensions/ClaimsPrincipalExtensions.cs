using System.Security.Claims;
using TheOneCRM.Domain.Models.Constants;

namespace TheOneCRM.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// يستخرج الـ User ID من الـ JWT
        /// </summary>
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// يستخرج الـ Email من الـ JWT
        /// </summary>
        public static string? GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }

        /// <summary>
        /// يستخرج الـ Role الأساسي للمستخدم حسب أولوية محددة
        /// (Marketing → Sales → Support → Admin)
        /// </summary>
        public static string? GetPrimaryRole(this ClaimsPrincipal user)
        {
            var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            if (roles.Contains(UserRoles.Marketing)) return UserRoles.Marketing;
            if (roles.Contains(UserRoles.Sales)) return UserRoles.Sales;
            if (roles.Contains(UserRoles.Support)) return UserRoles.Support;
            if (roles.Contains(UserRoles.Admin)) return UserRoles.Admin;

            return roles.FirstOrDefault();
        }

        /// <summary>
        /// يجيب كل الـ Roles بتاعة المستخدم
        /// </summary>
        public static List<string> GetAllRoles(this ClaimsPrincipal user)
        {
            return user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        }

        /// <summary>
        /// يتحقق إن المستخدم Admin
        /// </summary>
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(UserRoles.Admin);
        }
    }
    }
