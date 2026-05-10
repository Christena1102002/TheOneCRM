using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;

namespace TheOneCRM.Application.Interfaces
{
    public interface IMarketingService
    {
        Task<StatisticsMarketingDto> GetStatisticsAsync();
    }
}
