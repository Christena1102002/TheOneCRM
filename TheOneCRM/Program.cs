
//using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Extensions;
using TheOneCRM.API.Helper;
using TheOneCRM.API.Middlewares;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Application.Mapping;
using TheOneCRM.Application.Services.Auth;

//using TheOneCRM.Application.Services.Auth;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Infrastructure.Data;
using TheOneCRM.Infrastructure.Migrations;
namespace TheOneCRM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
          

            //builder.Configuration.GetSection("JwtSettings");
            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<DataSeader>();
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll", policy =>
            //    {
            //        policy.AllowAnyOrigin()
            //              .AllowAnyMethod()
            //              .AllowAnyHeader();
            //    });
            //});
          
            
            builder.Services.AddIdentityService(builder.Configuration);
            builder.Services.AddApplicationService();
            builder.Services.AddCustomCors(builder.Configuration);
         


            builder.Host.UseSerilog((ctx, lc) => lc
             .MinimumLevel.Information()
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
             .WriteTo.Console()
             .WriteTo.File(
                 path: Path.Combine(ctx.HostingEnvironment.ContentRootPath, "logs", "app-.log"),
                 rollingInterval: RollingInterval.Day,
                 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                 retainedFileCountLimit: 31)
            .Enrich.FromLogContext()
             .Enrich.WithMachineName()
            );


            var app = builder.Build();
          
            
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // 1) Apply migrations
                    var db = services.GetRequiredService<AppDbContext>();
                    
                    
                    
                    
                    
                    
                    await db.Database.MigrateAsync();

                    // 2) Run seeder
                    var seeder = services.GetRequiredService<DataSeader>();
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "❌ Error while migrating or seeding the database");
                    throw;
                }
            }
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

         
            app.UseSwagger();
            app.UseCors("CorsPolicy");
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard();
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                using var scope = app.Services.CreateScope();
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                recurringJobManager.AddOrUpdate<RefreshTokenCleanupJob>(
                    "refresh-token-cleanup",
                    job => job.ExecuteAsync(),
                    builder.Configuration.GetValue<string>("RefreshTokenCleanup:CronExpression") ?? Cron.Daily());
            });
            app.MapControllers();

            app.Run();
        }
    }
}
