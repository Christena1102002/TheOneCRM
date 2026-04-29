using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification;

namespace TheOneCRM.Application.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper= mapper;
        }

        //public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto)
        //{
        //    //var campaign = await _unitOfWork.Repository<Campaigns>()
        //    //    .GetEntityWithSpec(new CampaignByIdSpec(dto.CampaignId));

        //    //if (campaign == null)
        //        //return new ApiResponse<CustomerResponseDto>(404, "Campaign not found");

        //    var customer = _mapper.Map<Customer>(dto);

        //    //foreach (var serviceId in dto.ServiceIds)
        //    //{
        //    //    customer.customerServices.Add(new CustomerServices
        //    //    {
        //    //        ServiceId = serviceId
        //    //    });
        //    //}

        //    await _unitOfWork.Repository<Customer>().AddAsync(customer);
        //    await _unitOfWork.SaveChangesAsync();

        //    var result = _mapper.Map<CustomerResponseDto>(customer);
        //    return result;
        //    //return new ApiResponse<CustomerResponseDto>(201, data: result);
        //}
        public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (_mapper == null) throw new InvalidOperationException("Mapper is null");
            if (_unitOfWork == null) throw new InvalidOperationException("UnitOfWork is null");

            var customer = _mapper.Map<Customer>(dto);
            if (customer == null) throw new InvalidOperationException("Mapping returned null");

            var repo = _unitOfWork.Repository<Customer>();
            if (repo == null) throw new InvalidOperationException("Repository<Customer>() returned null");

            await repo.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<CustomerResponseDto>(customer);
            return result;
        }
    }
}
