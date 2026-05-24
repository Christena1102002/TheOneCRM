using TheOneCRM.Domain.Models.DTOs.Projects;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    // كل المشاريع النشطة (للأدمن)
    public class ActiveProjectsSpec : BaseSpecification<Projects>
    {
        public ActiveProjectsSpec(ProjectParams p) : base(x =>
            x.Status == StatusOfProject.InProgress &&
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
