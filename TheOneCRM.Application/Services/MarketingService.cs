using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification;
using TheOneCRM.Infrastructure.Specsification.CampaignsSpec;
using TheOneCRM.Infrastructure.Specsification.Customerspec;
using TheOneCRM.Infrastructure.Specsification.Source;

namespace TheOneCRM.Application.Services
{
    public class MarketingService : IMarketingService
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

        public async Task<MarketingDashboardDto> GetDashboardStats()
        {
            // الحملات النشطة
            var ActiveCampaigns = await _unitOfWork.Repository<Campaigns>().CountAsync(new ActiveCampaignsWithParamSpecification());
            // عملاء اليوم
            var TodayCustomers = await _unitOfWork.Repository<Customer>().CountAsync(new TodayCustomersSpecification());

            var totalCustomers = await _unitOfWork.Repository<Customer>().CountAsync(new AllCustomersSpecification());

            return new MarketingDashboardDto
            {
                //    CustomerCost = customerCost,
                ActiveCampaigns = ActiveCampaigns,
                TodayCustomers = TodayCustomers,
                TotalPotentialCustomers = totalCustomers
            };

        }
        public async Task<List<DailyLeadsDto>> GetPotentialCustomersLast7DaysAsync()
        {
            var customers = await _unitOfWork
           .Repository<Customer>()
           .ListWithSelectAsync(new PotentialCustomersLast7DaysSpecification(),c=>c.CreatedAt);

            var grouped=customers.GroupBy(d=>d.Date)
                .ToDictionary(g=>g.Key,g=>g.Count());
            // أول يوم في آخر 7 أيام (يشمل اليوم الحالي)
            var fromDate = DateTime.UtcNow.Date.AddDays(-6);

            // تجهيز النتيجة مع تضمين الأيام التي لا تحتوي على بيانات
            var result = Enumerable.Range(0, 7)
                .Select(i =>
                {
                    var date = fromDate.AddDays(i);

                    return new DailyLeadsDto
                    {
                        Date = date.ToString("yyyy-MM-dd"),
                        Count = grouped.ContainsKey(date)
                            ? grouped[date]
                            : 0
                    };
                })
                .ToList();

            return result;
        }

       public async Task<List<SourcePerformanceDto>> GetSourcePerformanceAsync()
        {
            var result = await _unitOfWork.Repository<Campaigns>()
                //.ListWithSelectAsync(new SourcePerformanceSpecification(),
                .ListWithSelectAsync(spec:null,
                c => new SourcePerformanceDto
                {
                    ChannelSourceId = c.ChannelSourceId,
                    SourceName = c.ChannelSource.Name,
                    CustomersCount = c.Customers.Count(),
                    TotalBudget = c.Budget
                });
            return result.ToList();
        }
    }
}
