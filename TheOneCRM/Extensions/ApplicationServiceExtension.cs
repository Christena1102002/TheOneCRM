using Microsoft.Extensions.DependencyInjection;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Application.Mapping;
//using TheOneCRM.Application.Services.Auth;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Infrastructure.Migrations;

namespace TheOneCRM.API.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //Services.AddScoped<IAuthService, AuthService>();

          
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
          Services.AddAutoMapper(typeof(MappingProfile));
         
           
         


            return Services;
        }
    }
}
