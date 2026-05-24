using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.TaskSpec
{
    // المهام النشطة (غير المكتملة) والمعيّنة لمطوّر — لحساب حمل العمل
    public class WorkloadTasksSpec : BaseSpecification<Tasks>
    {
        public WorkloadTasksSpec(int? projectId, PriorityStatus? priority) : base(x =>
            x.AssignedToId != null &&
            x.Status != StatusOfTask.Completed &&
            (!projectId.HasValue || x.ProjectId == projectId.Value) &&
            (!priority.HasValue || x.Priority == priority.Value)
        )
        {
        }
    }
}
