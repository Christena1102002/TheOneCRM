using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    }
}
