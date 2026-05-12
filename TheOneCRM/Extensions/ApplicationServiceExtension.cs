using Microsoft.Extensions.DependencyInjection;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Application.Interfaces.IDailyReport;
using TheOneCRM.Application.Interfaces.IServices;
using TheOneCRM.Application.Interfaces.ISourceService;
using TheOneCRM.Application.Mapping;
using TheOneCRM.Application.Services;
using TheOneCRM.Application.Services.Auth;
using TheOneCRM.Application.Services.Customers;
using TheOneCRM.Application.Services.Report;
using TheOneCRM.Application.Services.Services;
using TheOneCRM.Application.Services.Sources;
using TheOneCRM.Application.Services.Token;


//using TheOneCRM.Application.Services.Auth;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Migrations;

namespace TheOneCRM.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<ICustomerService, CustomerService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddAutoMapper(typeof(MappingProfile));
            Services.AddScoped<ICampaignService, CampaignService>();
            Services.AddScoped<ISourceService, SourceService>();
            Services.AddScoped<IServicesService, ServicesService>();
            Services.AddScoped<IMarketingService, MarketingService>();
            Services.AddScoped<IDailyReportService, DailyReportService>();
            return Services;
        }
    }
}
