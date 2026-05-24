using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    public class ProjectByIdSpec : BaseSpecification<Projects>
    {
        public ProjectByIdSpec(int id) : base(x => x.Id == id)
        {
            // مفيش includes — ProjectTo هيتولّى الموضوع
        }
    }
}
