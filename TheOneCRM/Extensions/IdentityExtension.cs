using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Data;
namespace TheOneCRM.API.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection Services, IConfiguration Configuration)
        {
            //Services.AddScoped<ITokenService, JwtTokenService>();

            Services.AddIdentity<AppUser, IdentityRole>()
          .AddEntityFrameworkStores<AppDbContext>()
          .AddDefaultTokenProviders();
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Configuration["JWT:Key"]!)),
                };
            });
   
            //Services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            //     // تعريف الـ JWT Bearer
            //     var securityScheme = new OpenApiSecurityScheme
            //     {
            //         Name = "Authorization",
            //         Description = "Enter the token in this format: 'Bearer {token}'",
            //         In = ParameterLocation.Header,
            //         Type = SecuritySchemeType.Http,
            //         Scheme = "bearer",
            //         BearerFormat = "JWT",
            //         Reference = new OpenApiReference
            //         {
            //             Type = ReferenceType.SecurityScheme,
            //             Id = "Bearer"
            //         }
            //     };
            //     c.EnableAnnotations();
            //     c.AddSecurityDefinition("Bearer", securityScheme);


            //     var securityRequirement = new OpenApiSecurityRequirement
            //         {
            //             { securityScheme, new string[] { } }
            //         };

            //     c.AddSecurityRequirement(securityRequirement);
            // });
            Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CRM API",
                    Version = "v1",
                    Description = "This is API for The One CRM",
                    Contact = new OpenApiContact
                    {
                        Name = "Christen Ayman ",
                        Email = "christenaayman631@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/christena-ayman-18b749306")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token like this: Bearer {your token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                 {
                   new OpenApiSecurityScheme
                   {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer"
                       }
                   },
                      new string[] {}
                   }
                 });
            });
            Services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
       .UseSimpleAssemblyNameTypeSerializer()
       .UseRecommendedSerializerSettings()
       .UseSqlServerStorage(
           Configuration.GetConnectionString("DefaultConnection"),
           new SqlServerStorageOptions
           {
               CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
               SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
               QueuePollInterval = TimeSpan.Zero,
               UseRecommendedIsolationLevel = true,
               DisableGlobalLocks = true
           }));
            return Services;
        }
    }
}
