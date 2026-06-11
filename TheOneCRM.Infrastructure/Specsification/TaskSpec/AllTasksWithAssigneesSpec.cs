using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.TaskSpec
{
    public class AllTasksWithAssigneesSpec : BaseSpecification<Tasks>
    {
        public AllTasksWithAssigneesSpec() : base()
        {
            AddInclude(x => x.Assignees);
        }
    }
}
