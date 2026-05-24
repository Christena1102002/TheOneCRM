using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.TaskSpec
{
    // كل مهام مطوّر معيّن (من غير paging) — للإحصائيات
    public class TasksByDeveloperSpec : BaseSpecification<Tasks>
    {
        public TasksByDeveloperSpec(string developerId)
            : base(x => x.AssignedToId == developerId)
        {
        }
    }
}
