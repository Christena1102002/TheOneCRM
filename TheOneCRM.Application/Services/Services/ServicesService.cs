using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Application.Interfaces.IServices;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.ServicesDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification;

namespace TheOneCRM.Application.Services.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServicesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto)
        {
            // 1) تحقق من عدم تكرار الاسم (عربي أو إنجليزي)
            var existing = await _unitOfWork.Repository<Service>()
                .GetEntityWithSpec(new ServiceByNameSpec(dto.NameAr.Trim(), dto.NameEn.Trim()));

            if (existing != null)
            {
                var dup = existing.NameAr == dto.NameAr.Trim()
                    ? $"يوجد خدمة بنفس الاسم العربي: {dto.NameAr}"
                    : $"A service with English name '{dto.NameEn}' already exists.";
                throw new InvalidOperationException(dup);
            }

            // 2) Map and add
            var service = _mapper.Map<Service>(dto);
            service.NameAr = dto.NameAr.Trim();
            service.NameEn = dto.NameEn.Trim();

            await _unitOfWork.Repository<Service>().AddAsync(service);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ServiceDto>(service);
            return result;
        }
        public async Task<ServiceDto> GetServiceByIdAsync(int id)
        {
            var service = await _unitOfWork.Repository<Service>()
                .GetEntityWithSpec(new ServicesByIdsSpec(id));

            if (service == null)
                throw new KeyNotFoundException($"Service with id {id} not found");

            return _mapper.Map<ServiceDto>(service);
        }
        public async Task<ServiceDto> UpdateServiceAsync(int id, UpdateServiceDto dto)
        {
            // 1) جيب الخدمة
            var service = await _unitOfWork.Repository<Service>()
                .GetEntityWithSpec(new ServicesByIdsSpec(id));

            if (service == null)
                throw new KeyNotFoundException($"Service with id {id} not found");

            // 2) تحقق من عدم تكرار الاسم مع خدمة تانية
            var duplicate = await _unitOfWork.Repository<Service>()
                .GetEntityWithSpec(new ServiceByNameSpec(
                    dto.NameAr.Trim(), dto.NameEn.Trim(), id));

            if (duplicate != null)
            {
                var msg = duplicate.NameAr == dto.NameAr.Trim()
                    ? $"يوجد خدمة بنفس الاسم العربي: {dto.NameAr}"
                    : $"A service with English name '{dto.NameEn}' already exists.";
                throw new InvalidOperationException(msg);
            }

            // 3) تحديث القيم
            service.NameAr = dto.NameAr.Trim();
            service.NameEn = dto.NameEn.Trim();

            _unitOfWork.Repository<Service>().Update(service);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ServiceDto>(service);
        }
        public async Task DeleteServiceAsync(int id)
        {
            // 1) جيب الخدمة مع العملاء المرتبطين بيها
            var service = await _unitOfWork.Repository<Service>()
                .GetEntityWithSpec(new ServiceByIdWithCustomersSpec(id));

            if (service == null)
                throw new KeyNotFoundException($"Service with id {id} not found");

            // 2) تحقق إن مفيش عملاء مرتبطين بالخدمة
            if (service.customerServices != null && service.customerServices.Any())
                throw new InvalidOperationException(
                    $"لا يمكن حذف الخدمة '{service.NameAr}' لأنها مرتبطة بـ {service.customerServices.Count} عميل");

            // 3) احذف
            _unitOfWork.Repository<Service>().Delete(service);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<Pagination<ServiceDto>> GetAllServicesAsync(ServiceQueryParams queryParams)
        {
          
            var spec = new ServicesFilterSpec(queryParams);
            var services = await _unitOfWork.Repository<Service>().ListAsync(spec);

            var countSpec = new ServicesFilterCountSpec(queryParams);
            var totalCount = await _unitOfWork.Repository<Service>().CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<ServiceDto>>(services);

            return new Pagination<ServiceDto>(
                queryParams.PageIndex,
                queryParams.PageSize,
                totalCount,
                data);
        }
    }

}