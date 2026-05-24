using System.Linq;
using TheOneCRM.Domain.Models.DTOs.Projects;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    // مشاريع مهندس معيّن (الـ developer بيشوف مشاريعه بس)
    public class ProjectsByEngineerSpec : BaseSpecification<Projects>
    {
        public ProjectsByEngineerSpec(ProjectParams p, string engineerId) : base(x =>
            x.ProjectEngineers.Any(pe => pe.EngineerId == engineerId) &&
            (string.IsNullOrEmpty(p.Search) ||
             x.Title.Contains(p.Search) ||
             (x.Customer != null && x.Customer.FullName.Contains(p.Search)) ||
             (x.Customer != null && x.Customer.CampanyName != null && x.Customer.CampanyName.Contains(p.Search)))
        )
        {
            ApplyOrderByDescending(x => x.CreatedAt);
            ApplyPaging((p.PageIndex - 1) * p.PageSize, p.PageSize);
        }
    }
}
