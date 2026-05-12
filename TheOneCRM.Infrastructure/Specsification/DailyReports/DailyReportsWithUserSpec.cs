using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.DTOs.DailyReports;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.DailyReports
{
    public class DailyReportsWithUserSpec : BaseSpecification<DailyReport>
    {
        // كل التقارير مع الفلترة والبيجنيشن
        public DailyReportsWithUserSpec(DailyReportQueryParams p) : base(x =>
            (string.IsNullOrEmpty(p.UserId) || x.UserId == p.UserId) &&
            (!p.FromDate.HasValue || x.ReportDate >= p.FromDate.Value.Date) &&
            (!p.ToDate.HasValue || x.ReportDate <= p.ToDate.Value.Date) &&
            (string.IsNullOrEmpty(p.Search) ||
                x.CompletedTasks.Contains(p.Search) ||
                (x.TasksInProgress != null && x.TasksInProgress.Contains(p.Search)) ||
                (x.PlannedTasks != null && x.PlannedTasks.Contains(p.Search)) ||
                (x.Challenges != null && x.Challenges.Contains(p.Search)))
        )
        {
            AddInclude(x => x.appUser);
            ApplyPaging(p.PageSize * (p.PageIndex - 1), p.PageSize);

            switch (p.Sort)
            {
                case "dateAsc": ApplyOrderBy(x => x.ReportDate); break;
                case "hoursAsc": ApplyOrderBy(x => x.WorkHours); break;
                case "hoursDesc": ApplyOrderByDescending(x => x.WorkHours); break;
                default: ApplyOrderByDescending(x => x.ReportDate); break; // الأحدث أولاً
            }
        }
        public DailyReportsWithUserSpec(DailyReportQueryParams p, bool count) : base(x =>
           (string.IsNullOrEmpty(p.UserId) || x.UserId == p.UserId) &&
           (!p.FromDate.HasValue || x.ReportDate >= p.FromDate.Value.Date) &&
           (!p.ToDate.HasValue || x.ReportDate <= p.ToDate.Value.Date) &&
           (string.IsNullOrEmpty(p.Search) ||
               x.CompletedTasks.Contains(p.Search) ||
               (x.TasksInProgress != null && x.TasksInProgress.Contains(p.Search)) ||
               (x.PlannedTasks != null && x.PlannedTasks.Contains(p.Search)) ||
               (x.Challenges != null && x.Challenges.Contains(p.Search)))
       )
        { }

        // تقرير واحد بالـ Id مع بيانات الموظف
        public DailyReportsWithUserSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.appUser);
        }

        // تقرير بتاريخ معين لموظف معين (للتحقق من التكرار)
        public DailyReportsWithUserSpec(string userId, DateTime date)
            : base(x => x.UserId == userId && x.ReportDate.Date == date.Date)
        { }
    }
}
