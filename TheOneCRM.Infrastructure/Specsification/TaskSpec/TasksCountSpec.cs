using TheOneCRM.Domain.Models.DTOs.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.TaskSpec
{
    public class TasksCountSpec : BaseSpecification<Tasks>
    {
        public TasksCountSpec(TaskParams p, string? assignedToId = null) : base(x =>
            (assignedToId == null || x.AssignedToId == assignedToId) &&
            (!p.ProjectId.HasValue || x.ProjectId == p.ProjectId.Value) &&
            (!p.Status.HasValue || x.Status == p.Status.Value) &&
            (!p.Priority.HasValue || x.Priority == p.Priority.Value) &&
            (string.IsNullOrEmpty(p.Search) ||
             x.Title.Contains(p.Search) ||
             (x.Description != null && x.Description.Contains(p.Search)))
        )
        { }
    }
}
