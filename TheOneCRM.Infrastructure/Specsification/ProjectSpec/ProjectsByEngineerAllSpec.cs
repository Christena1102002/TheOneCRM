using System.Linq;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    // كل مشاريع مهندس معيّن (من غير paging) — للإحصائيات
    public class ProjectsByEngineerAllSpec : BaseSpecification<Projects>
    {
        public ProjectsByEngineerAllSpec(string engineerId)
            : base(x => x.ProjectEngineers.Any(pe => pe.EngineerId == engineerId))
        {
        }
    }
}
