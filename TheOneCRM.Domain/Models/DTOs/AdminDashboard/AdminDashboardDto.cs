namespace TheOneCRM.Domain.Models.DTOs.AdminDashboard
{
    public class AdminDashboardDto
    {
        public DashboardCardDto Buyers { get; set; } = new();          // العملاء المشتريين
        public DashboardCardDto NonBuyers { get; set; } = new();       // العملاء غير المشتريين
        public DashboardCardDto ActiveProjects { get; set; } = new();  // المشاريع النشطة
        public DashboardCardDto PendingTasks { get; set; } = new();    // المهام المعلقة

        // إيرادات العقود
        public ContractsRevenueDto ContractsRevenue { get; set; } = new();

        // عدد العملاء المشتريين شهرياً (مجمّع من كل السيلز)
        public List<MonthlyBuyersDto> MonthlyBuyers { get; set; } = new();

        // قيمة العقود شهرياً (مجموع الـ Price لعقود اتعملت في كل شهر)
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    }

    public class MonthlyBuyersDto
    {
        public int Year { get; set; }
        public int Month { get; set; }              // 1..12
        public string MonthName { get; set; } = null!;  // يناير، فبراير، ...
        public int Count { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Month { get; set; }              // 1..12
        public string MonthName { get; set; } = null!;  // يناير، فبراير، ...
        public decimal Revenue { get; set; }
    }

    public class DashboardCardDto
    {
        public int Count { get; set; }
        public int ChangePercent { get; set; }   // النسبة مقارنة بالشهر الماضي
    }

    public class ContractsRevenueDto
    {
        public decimal TotalValue { get; set; }     // إجمالي قيمة كل العقود في النظام
        public decimal ActiveValue { get; set; }    // قيمة العقود النشطة (سارية حالياً)
        public decimal AverageValue { get; set; }   // متوسط قيمة العقد الواحد
        public int ActiveCount { get; set; }        // عدد العقود النشطة (للـ badge)
    }
}
