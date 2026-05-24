using System.Linq;
using TheOneCRM.Domain.Models.DTOs.Projects;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    public class ProjectsByEngineerCountSpec : BaseSpecification<Projects>
    {
        public ProjectsByEngineerCountSpec(ProjectParams p, string engineerId) : base(x =>
            x.ProjectEngineers.Any(pe => pe.EngineerId == engineerId) &&
            (string.IsNullOrEmpty(p.Search) ||
             x.Title.Contains(p.Search) ||
             (x.Customer != null && x.Customer.FullName.Contains(p.Search)) ||
             (x.Customer != null && x.Customer.CampanyName != null && x.Customer.CampanyName.Contains(p.Search)))
        )
        { }
    }
}
