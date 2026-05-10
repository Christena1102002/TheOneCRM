using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces.ISourceService;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Services.Sources
{
    public class SourceService : ISourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ChannelSourceDto> CreateChannelSourceAsync(CreateChannelSourceDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("Name is required");
            // ممكن نمنع التكرار
            var exists = await _unitOfWork.Repository<ChannelSource>()
                .AnyAsync(x => x.Name == dto.Name);

            if (exists)
                throw new InvalidOperationException("ChannelSource already exists");

            var source = _mapper.Map<ChannelSource>(dto);

            await _unitOfWork.Repository<ChannelSource>().AddAsync(source);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ChannelSourceDto>(source);
        }
        public async Task<IReadOnlyList<ChannelSourceDto>> GetAllChannelSourcesAsync()
        {
            var sources = await _unitOfWork.Repository<ChannelSource>().ListAllAsync();
            return _mapper.Map<IReadOnlyList<ChannelSourceDto>>(sources);
        }
        //public async Task<List<SourceStatisticsDto>> GetSourceStatisticsAsync()
        // {
        //     // إنشاء الـ Specification
        //     var spec = new CustomersWithCampaignAndSourceSpecification();

        //     // جلب كل العملاء مع Campaign و Source
        //     var customers = await _unitOfWork.Repository<Customer>()
        //         .GetAllWithSpecAsync(spec);

        //     // Group By Source
        //     var result = customers
        //         .GroupBy(c => new
        //         {
        //             SourceId = c.Campaign.Source.Id,
        //             SourceName = c.Campaign.Source.Name
        //         })
        //         .Select(g => new SourceStatisticsDto
        //         {
        //             SourceId = g.Key.SourceId,
        //             SourceName = g.Key.SourceName,

        //             // عدد الـ Buyers
        //             BuyersCount = g.Count(c => c.Status == CustomerStatus.Buyer),

        //             // عدد الـ NotBuyers
        //             NotBuyersCount = g.Count(c => c.Status == CustomerStatus.NotBuyer),

        //             // إجمالي العملاء
        //             TotalCustomers = g.Count()
        //         })
        //         .ToList();

        //     return result;
        // }
    }
}
