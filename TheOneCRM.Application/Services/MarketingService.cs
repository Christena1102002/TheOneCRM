using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification;

namespace TheOneCRM.Application.Services
{
    public class MarketingService :IMarketingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MarketingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StatisticsMarketingDto> GetStatisticsAsync()
        {
            var customerRepo = _unitOfWork.Repository<Customer>();
            // COUNT(*) FROM Customers
            var totalCustomers =
                await customerRepo.CountAsync(new AllCustomersSpecification());

            var buyerCustomers =
              await customerRepo.CountAsync(new BuyerCustomersSpecification());

            var notBuyerCustomers =
              await customerRepo.CountAsync(new NotBuyerCustomersSpecification());


            var conversionRate = totalCustomers == 0
               ? 0
               : Math.Round((decimal)buyerCustomers / totalCustomers * 100, 2);

            return new StatisticsMarketingDto
            {
                TotalCustomers = totalCustomers,
                BuyerCustomers = buyerCustomers,
                NotBuyerCustomers = notBuyerCustomers,
                ConversionRate = conversionRate
            };



        }
    }
}
