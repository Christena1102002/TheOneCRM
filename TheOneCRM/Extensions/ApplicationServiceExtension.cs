using Microsoft.Extensions.DependencyInjection;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Application.Mapping;
using TheOneCRM.Application.Services.Customers;

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
            //Services.AddScoped<IAuthService, AuthService>();
            
                Services.AddScoped<ICustomerService, CustomerService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
          Services.AddAutoMapper(typeof(MappingProfile));
         
           
         


            return Services;
        }
    }
}
