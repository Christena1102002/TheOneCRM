using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.TaskSpec
{
    // مهمة واحدة بشرط إنها معيّنة للـ developer ده
    public class TaskByIdForDeveloperSpec : BaseSpecification<Tasks>
    {
        public TaskByIdForDeveloperSpec(int id, string developerId)
            : base(x => x.Id == id && x.AssignedToId == developerId)
        {
        }
    }
}
