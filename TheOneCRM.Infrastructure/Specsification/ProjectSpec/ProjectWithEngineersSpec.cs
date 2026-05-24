using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    public class ProjectWithEngineersSpec : BaseSpecification<Projects>
    {
        public ProjectWithEngineersSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProjectEngineers);
        }
    }
}
