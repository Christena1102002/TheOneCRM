using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ProjectSpec
{
    // كل المشاريع (لو engineerId = null) أو مشاريع مطوّر معيّن بس
    public class ProjectsForDropdownSpec : BaseSpecification<Projects>
    {
        public ProjectsForDropdownSpec(string? engineerId)
            : base(p => engineerId == null ||
                        p.ProjectEngineers.Any(pe => pe.EngineerId == engineerId))
        {
            ApplyOrderByDescending(p => p.CreatedAt);
        }
    }
}
