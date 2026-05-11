using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.API.Helper
{
    public class DataSeader
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public DataSeader(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task SeedAsync()
        {
            // جلب إعدادات الإداري من appsettings.json

            //var adminEmail = "admin@Jeebly.com";
            //var adminPassword = "Admin@1234";
            var adminEmail = _configuration["AdminUser:Email"];
            var adminPassword = _configuration["AdminUser:Password"];
          

            var roles = _configuration.GetSection("Roles").Get<List<string>>();
            foreach(var role in roles)
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                  await  _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new AppUser
                {
                    UserName = adminEmail.Split("@")[0],
                    Email = adminEmail,
                    NormalizedEmail = adminEmail,
                    NormalizedUserName = adminEmail.Split("@")[0].ToUpper(),
                  
                    EmailConfirmed = true // لتجنب الحاجة إلى تأكيد البريد
                };

                var result = await _userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    // إضافة دور "Admin" للمستخدم
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}



