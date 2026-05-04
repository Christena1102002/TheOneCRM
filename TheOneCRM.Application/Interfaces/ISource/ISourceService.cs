using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;

namespace TheOneCRM.Application.Interfaces.ISourceService
{
    public interface ISourceService
    {
        Task<ChannelSourceDto> CreateChannelSourceAsync(CreateChannelSourceDto dto);
        Task<IReadOnlyList<ChannelSourceDto>> GetAllChannelSourcesAsync();
    }
}
