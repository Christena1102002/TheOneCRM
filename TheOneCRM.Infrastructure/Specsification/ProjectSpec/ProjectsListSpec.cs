using TheOneCRM.Domain.Models.DTOs.Projects;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    public class ProjectsListSpec : BaseSpecification<Projects>
    {
        public ProjectsListSpec(ProjectParams p) : base(x =>
            string.IsNullOrEmpty(p.Search) ||
            x.Title.Contains(p.Search) ||
            (x.Customer != null && x.Customer.FullName.Contains(p.Search)) ||
            (x.Customer != null && x.Customer.CampanyName != null && x.Customer.CampanyName.Contains(p.Search))
        )
        {
            ApplyOrderByDescending(x => x.CreatedAt);
            ApplyPaging((p.PageIndex - 1) * p.PageSize, p.PageSize);
        }
    }
}
