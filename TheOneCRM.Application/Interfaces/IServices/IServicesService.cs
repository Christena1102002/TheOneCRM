using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.ServicesDtos;

namespace TheOneCRM.Application.Interfaces.IServices
{
    public interface IServicesService
    {
        Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto);
        Task<ServiceDto> GetServiceByIdAsync(int id);
        Task<ServiceDto> UpdateServiceAsync(int id, UpdateServiceDto dto);
        Task DeleteServiceAsync(int id);
        Task<Pagination<ServiceDto>> GetAllServicesAsync(ServiceQueryParams queryParams);
    }
}
