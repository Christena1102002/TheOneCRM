using System.Linq;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    // مشروع واحد بشرط إن المهندس معيّن عليه
    public class ProjectByIdForEngineerSpec : BaseSpecification<Projects>
    {
        public ProjectByIdForEngineerSpec(int id, string engineerId)
            : base(x => x.Id == id && x.ProjectEngineers.Any(pe => pe.EngineerId == engineerId))
        {
        }
    }
}
