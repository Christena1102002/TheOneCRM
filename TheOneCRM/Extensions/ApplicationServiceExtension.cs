using Microsoft.Extensions.DependencyInjection;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Application.Interfaces.IAnalytics;
using TheOneCRM.Application.Interfaces.IAppointment;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Application.Interfaces.IContracts;
using TheOneCRM.Application.Interfaces.ICountry;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Application.Interfaces.IDailyReport;
using TheOneCRM.Application.Interfaces.IPriceQuotation;
using TheOneCRM.Application.Interfaces.IProjects;
using TheOneCRM.Application.Interfaces.IResources;
using TheOneCRM.Application.Interfaces.ITasks;
using TheOneCRM.Application.Interfaces.IServices;
using TheOneCRM.Application.Interfaces.ISourceService;
using TheOneCRM.Application.Interfaces.ISupportTickets;
using TheOneCRM.Application.Mapping;
using TheOneCRM.Application.Services;
using TheOneCRM.Application.Services.AppointmentS;
using TheOneCRM.Application.Services.Auth;
using TheOneCRM.Application.Services.Contracts;
using TheOneCRM.Application.Services.Country;
using TheOneCRM.Application.Services.Customers;
using TheOneCRM.Application.Services.price;
using TheOneCRM.Application.Services.Analytics;
using TheOneCRM.Application.Services.Projects;
using TheOneCRM.Application.Services.Resources;
using TheOneCRM.Application.Services.Tasks;
using TheOneCRM.Application.Services.Report;
using TheOneCRM.Application.Services.Services;
using TheOneCRM.Application.Services.Sources;
using TheOneCRM.Application.Services.Tickets;
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
            Services.AddScoped<IPriceQuotationService, PriceQuotationService>();
            Services.AddScoped<ICountryService, CountryService>();
            Services.AddScoped<IContractService, ContractService>();
            Services.AddScoped<IAppointmentService, AppointmentService>();
            Services.AddScoped<ISupportTicketService, SupportTicketService>();
            Services.AddScoped<IProjectService, ProjectService>();
            Services.AddScoped<ITaskService, TaskService>();
            Services.AddScoped<IResourceManagementService, ResourceManagementService>();
            Services.AddScoped<IDeveloperAnalyticsService, DeveloperAnalyticsService>();

            return Services;
        }
    }
}
