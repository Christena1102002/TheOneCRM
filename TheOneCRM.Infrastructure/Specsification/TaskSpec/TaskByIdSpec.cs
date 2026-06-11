using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.TaskSpec
{
    public class TaskByIdSpec : BaseSpecification<Tasks>
    {
        public TaskByIdSpec(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Assignees);
        }
    }
}
