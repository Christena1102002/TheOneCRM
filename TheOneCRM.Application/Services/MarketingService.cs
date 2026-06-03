using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.DTOs.SourceDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
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

        public async Task<StatisticsMarketingDto> GetStatisticsAsync(string? ownerId)
        {
            // الأدمن: كل العملاء (ownerId = null). الماركتينج: اللي هو ضافهم بس
            var all = await _unitOfWork.Repository<Customer>().ListAllAsync();
            var customers = ownerId == null
                ? all
                : all.Where(c => c.CreatedById == ownerId).ToList();

            var totalCustomers = customers.Count;
            var buyerCustomers = customers.Count(c => c.status == StatusOfCustomers.Buyer);
            var notBuyerCustomers = customers.Count(c => c.status == StatusOfCustomers.NotBuyer);

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

        public async Task<MarketingDashboardDto> GetDashboardStats(string? ownerId)
        {
            var today = DateTime.UtcNow.Date;

            // الحملات (الأدمن: الكل، الماركتينج: حملاته)
            var allCampaigns = await _unitOfWork.Repository<Campaigns>().ListAllAsync();
            var campaigns = ownerId == null
                ? allCampaigns
                : allCampaigns.Where(cm => cm.AppUserId == ownerId).ToList();
            var activeCampaigns = campaigns.Count(cm => cm.Status == CampaignStatus.Active);

            // العملاء (الأدمن: الكل، الماركتينج: اللي هو ضافهم)
            var allCustomers = await _unitOfWork.Repository<Customer>().ListAllAsync();
            var customers = ownerId == null
                ? allCustomers
                : allCustomers.Where(c => c.CreatedById == ownerId).ToList();

            var todayCustomers = customers.Count(c => c.CreatedAt.Date == today);
            var totalCustomers = customers.Count;

            // تكلفة العميل = مجموع (الميزانية اليومية لكل حملة) ÷ عدد عملاء اليوم
            var totalDailyBudget = campaigns
                .Where(cm => cm.DurationDays > 0)
                .Sum(cm => cm.Budget / cm.DurationDays);

            var customerCost = todayCustomers > 0
                ? Math.Round(totalDailyBudget / todayCustomers, 2)
                : 0;

            return new MarketingDashboardDto
            {
                CustomerCost = customerCost,
                ActiveCampaigns = activeCampaigns,
                TodayCustomers = todayCustomers,
                TotalPotentialCustomers = totalCustomers
            };
        }
        public async Task<List<DailyLeadsDto>> GetPotentialCustomersLast7DaysAsync(string? ownerId)
        {
            var all = await _unitOfWork
           .Repository<Customer>()
           .ListAsync(new PotentialCustomersLast7DaysSpecification());

            // الأدمن: الكل، الماركتينج: اللي هو ضافهم
            var filtered = ownerId == null
                ? all.Select(c => c.CreatedAt).ToList()
                : all.Where(c => c.CreatedById == ownerId).Select(c => c.CreatedAt).ToList();

            var grouped = filtered.GroupBy(d => d.Date)
                .ToDictionary(g => g.Key, g => g.Count());
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

       public async Task<List<SourcePerformanceDto>> GetSourcePerformanceAsync(string? ownerId)
        {
            // الأدمن: كل الحملات، الماركتينج: حملاته هو بس
            var allCampaigns = await _unitOfWork.Repository<Campaigns>().ListAsync(new CampaignsSpec(ownerId));

            // جيبي كل العملاء المرتبطين بحملات وعدّيهم لكل حملة
            var allCustomers = await _unitOfWork.Repository<Customer>().ListAllAsync();
            var customersByCampaign = allCustomers
                .Where(x => x.compaignId.HasValue)
                .GroupBy(x => x.compaignId!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            return allCampaigns.Select(c => new SourcePerformanceDto
            {
                ChannelSourceId = c.ChannelSourceId,
                SourceName = c.ChannelSource?.Name ?? string.Empty,
                CustomersCount = customersByCampaign.TryGetValue(c.Id, out var cnt) ? cnt : 0,
                TotalBudget = c.Budget
            }).ToList();
        }
    }
}
