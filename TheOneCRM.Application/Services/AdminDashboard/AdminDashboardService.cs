using System;
using System.Linq;
using System.Threading.Tasks;
using TheOneCRM.Application.Interfaces.IAdminDashboard;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.AdminDashboard;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TaskEntity = TheOneCRM.Domain.Models.Entities.Tasks;
using ProjectEntity = TheOneCRM.Domain.Models.Entities.Projects;

namespace TheOneCRM.Application.Services.AdminDashboard
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var startOfThisMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            // العملاء
            var customers = await _unitOfWork.Repository<Customer>().ListAllAsync();
            var buyersNow = customers.Count(c => c.status == CustomerStatus.Buyer);
            var buyersBefore = customers.Count(c => c.status == CustomerStatus.Buyer && c.CreatedAt < startOfThisMonth);
            var nonBuyersNow = customers.Count(c => c.status == CustomerStatus.NotBuyer);
            var nonBuyersBefore = customers.Count(c => c.status == CustomerStatus.NotBuyer && c.CreatedAt < startOfThisMonth);

            // المشاريع
            var projects = await _unitOfWork.Repository<ProjectEntity>().ListAllAsync();
            var activeNow = projects.Count(p => p.Status == StatusOfProject.InProgress);
            var activeBefore = projects.Count(p => p.Status == StatusOfProject.InProgress && p.CreatedAt < startOfThisMonth);

            // المهام المعلقة (أي حاجة مش Completed)
            var tasks = await _unitOfWork.Repository<TaskEntity>().ListAllAsync();
            var pendingNow = tasks.Count(t => t.Status != StatusOfTask.Completed);
            var pendingBefore = tasks.Count(t => t.Status != StatusOfTask.Completed && t.CreatedAt < startOfThisMonth);

            // إيرادات العقود
            var contracts = await _unitOfWork.Repository<Contract>().ListAllAsync();
            var activeContracts = contracts.Where(ct => ct.Status == ContractStatus.Active).ToList();
            var totalValue = contracts.Sum(ct => ct.Price);
            var activeValue = activeContracts.Sum(ct => ct.Price);
            var averageValue = contracts.Any() ? Math.Round(totalValue / contracts.Count, 2) : 0;

            // عدد المشتريين شهرياً (من بداية السنة الحالية لحد الشهر الحالي)
            var monthlyBuyers = BuildMonthlyBuyers(customers, DateTime.UtcNow);

            // إيرادات العقود شهرياً
            var monthlyRevenue = BuildMonthlyRevenue(contracts, DateTime.UtcNow);

            return new AdminDashboardDto
            {
                Buyers = new DashboardCardDto { Count = buyersNow, ChangePercent = ChangePercent(buyersNow, buyersBefore) },
                NonBuyers = new DashboardCardDto { Count = nonBuyersNow, ChangePercent = ChangePercent(nonBuyersNow, nonBuyersBefore) },
                ActiveProjects = new DashboardCardDto { Count = activeNow, ChangePercent = ChangePercent(activeNow, activeBefore) },
                PendingTasks = new DashboardCardDto { Count = pendingNow, ChangePercent = ChangePercent(pendingNow, pendingBefore) },
                ContractsRevenue = new ContractsRevenueDto
                {
                    TotalValue = totalValue,
                    ActiveValue = activeValue,
                    AverageValue = averageValue,
                    ActiveCount = activeContracts.Count
                },
                MonthlyBuyers = monthlyBuyers,
                MonthlyRevenue = monthlyRevenue
            };
        }

        // قيمة العقود شهرياً — مجموع Price للعقود اللي اتعملت في كل شهر
        private static List<MonthlyRevenueDto> BuildMonthlyRevenue(
            System.Collections.Generic.IReadOnlyList<Contract> contracts, DateTime now)
        {
            var result = new List<MonthlyRevenueDto>();
            for (int month = 1; month <= now.Month; month++)
            {
                var revenue = contracts
                    .Where(ct => ct.CreatedAt.Year == now.Year && ct.CreatedAt.Month == month)
                    .Sum(ct => ct.Price);

                result.Add(new MonthlyRevenueDto
                {
                    Year = now.Year,
                    Month = month,
                    MonthName = ArabicMonthName(month),
                    Revenue = revenue
                });
            }
            return result;
        }

        // عدد العملاء المشتريين شهرياً (من يناير لحد الشهر الحالي)
        private static List<MonthlyBuyersDto> BuildMonthlyBuyers(
            System.Collections.Generic.IReadOnlyList<Customer> customers, DateTime now)
        {
            var result = new List<MonthlyBuyersDto>();
            for (int month = 1; month <= now.Month; month++)
            {
                var count = customers.Count(c =>
                    c.status == CustomerStatus.Buyer &&
                    EffectiveDate(c).Year == now.Year &&
                    EffectiveDate(c).Month == month);

                result.Add(new MonthlyBuyersDto
                {
                    Year = now.Year,
                    Month = month,
                    MonthName = ArabicMonthName(month),
                    Count = count
                });
            }
            return result;
        }

        // التاريخ المعتبر للعميل: UpdatedAt لو موجود (آخر تعديل = ساعة ما الحالة اتغيرت لـ Buyer)
        // وإلا CreatedAt (للعملاء اللي لسه جداد ومتعدّلوش)
        private static DateTime EffectiveDate(Customer c) => c.UpdatedAt ?? c.CreatedAt;

        private static string ArabicMonthName(int month) => month switch
        {
            1 => "يناير",
            2 => "فبراير",
            3 => "مارس",
            4 => "أبريل",
            5 => "مايو",
            6 => "يونيو",
            7 => "يوليو",
            8 => "أغسطس",
            9 => "سبتمبر",
            10 => "أكتوبر",
            11 => "نوفمبر",
            12 => "ديسمبر",
            _ => ""
        };

        private static int ChangePercent(int current, int previous)
        {
            if (previous == 0) return current > 0 ? 100 : 0;
            return (int)Math.Round((current - previous) * 100.0 / previous);
        }
    }
}
