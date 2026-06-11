using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces.IContracts;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.Contracts;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification.ContracSpec;

namespace TheOneCRM.Application.Services.Contracts
{
    public class ContractService : IContractService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public ContractService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateContractAsync(CreateContractDto dto, string userId)
        {
            // 1) العميل موجود؟
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(dto.CustomerId);
            if (customer is null)
                throw new KeyNotFoundException($"Customer {dto.CustomerId} not found");

            // 2) validation الحالة - لو الفرونت بعت قيمة غلط (0 أو غير معرّفة)، حطها Active
            if (!Enum.IsDefined(typeof(ContractStatus), dto.Status))
                dto.Status = ContractStatus.Active;

            // 3) validation التواريخ
            if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
                throw new InvalidOperationException("End date cannot be before start date");
            var contract = _mapper.Map<Contract>(dto);
            contract.CreatedById = userId;
            _unitOfWork.Repository<Contract>().AddAsync(contract);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<CustomerLookupDto> GetCustomerLookupAsync(int customerId)
        {
           var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            if (customer is null)
                throw new KeyNotFoundException($"Customer {customerId} not found");
            return _mapper.Map<CustomerLookupDto>(customer);
       

        }
        public async Task<ContractResponseDto> GetContractByIdAsync(int id, string? ownerId)
        {
            var spec = new ContractByIdSpec(id);
            var contract = await _unitOfWork.Repository<Contract>().GetQueryableWithSpec(spec).ProjectTo<ContractResponseDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

            if (contract is null)
                throw new KeyNotFoundException($"Contract {id} not found");

            // السيلز يشوف العقد لو هو اللي أنشأه (ownerId != null)، الأدمن يشوف أي عقد (ownerId = null)
            if (ownerId != null && contract.CreatedById != ownerId)
                throw new UnauthorizedAccessException("This contract does not belong to you");

            return contract;
        }

        public async Task<Pagination<ContractResponseDto>> GetContractsAsync(ContractParams p, string? ownerId)
        {
            var listSpec = new ContractsListSpec(p, ownerId);
            var countSpec = new ContractsCountSpec(p, ownerId);

            var items = await _unitOfWork.Repository<Contract>()
                .GetQueryableWithSpec(listSpec)
                .ProjectTo<ContractResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var count = await _unitOfWork.Repository<Contract>().CountAsync(countSpec);

            return new Pagination<ContractResponseDto>(p.PageIndex, p.PageSize, count, items);
        }
        public async Task UpdateContractAsync(int id, UpdateContractDto dto, string? ownerId)
        {
            // 1) جلب العقد
            var contract = await _unitOfWork.Repository<Contract>().GetByIdAsync(id);
            if (contract is null)
                throw new KeyNotFoundException($"Contract {id} not found");

            // 2) السيلز يعدّل عقده هو بس (ownerId != null)، الأدمن يعدّل أي عقد (ownerId = null)
            if (ownerId != null && contract.CreatedById != ownerId)
                throw new UnauthorizedAccessException("This contract does not belong to you");

            // 3) validation الحالة - بس لو بعتها صراحةً
            if (dto.Status.HasValue)
            {
                if (!Enum.IsDefined(typeof(ContractStatus), dto.Status.Value))
                    throw new InvalidOperationException(
                        $"Invalid contract status. Allowed values: Active, Expired, Cancelled");

                contract.Status = dto.Status.Value;
            }

            // 4) validation التواريخ
            if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
                throw new InvalidOperationException("End date cannot be before start date");

            // 5) Map (Status متجاهل في الـ mapping - اتعمل فوق يدوياً)
            _mapper.Map(dto, contract);

            // 6) حفظ
            _unitOfWork.Repository<Contract>().Update(contract);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteContractAsync(int id)
        {
            var contract = await _unitOfWork.Repository<Contract>().GetByIdAsync(id);
            if (contract is null)
                throw new KeyNotFoundException($"Contract {id} not found");

            _unitOfWork.Repository<Contract>().Delete(contract);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateExpiredContractsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var spec = new ExpiredContractsSpecification(today);

            var expiredContracts = await _unitOfWork.Repository<Contract>()
                .GetQueryableWithSpec(spec)
                .ToListAsync();

            foreach (var contract in expiredContracts)
            {
                contract.Status = ContractStatus.Expired;
            }

            if (expiredContracts.Any())
                await _unitOfWork.SaveChangesAsync();
        }
        public async Task<ContractStatisticsDto> GetContractStatisticsAsync(string? ownerId)
        {
            // أولاً: تحديث العقود المنتهية
            await UpdateExpiredContractsAsync();

            var today = DateTime.UtcNow.Date;
            var after30Days = today.AddDays(30);

            // الأدمن: كل العقود. السيلز: عقوده هو بس (CreatedById)
            var all = await _unitOfWork.Repository<Contract>().ListAllAsync();
            var contracts = ownerId == null
                ? all
                : all.Where(c => c.CreatedById == ownerId).ToList();

            return new ContractStatisticsDto
            {
                TotalContracts = contracts.Count,
                ActiveContracts = contracts.Count(c => c.Status == ContractStatus.Active),
                ExpiredContracts = contracts.Count(c => c.Status == ContractStatus.Expired),
                ExpiringSoonContracts = contracts.Count(c =>
                    c.Status == ContractStatus.Active &&
                    c.EndDate.HasValue &&
                    c.EndDate.Value.Date >= today &&
                    c.EndDate.Value.Date <= after30Days)
            };
        }

    }
}
