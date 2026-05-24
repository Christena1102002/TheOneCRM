using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Resources
{
    public enum WorkloadSort
    {
        WorkloadDesc = 0, // حمل العمل (الأعلى أولاً) — الافتراضي
        WorkloadAsc = 1,
        TasksDesc = 2,
        NameAsc = 3
    }

    public class WorkloadParams
    {
        public int? ProjectId { get; set; }            // فلترة بالمشروع
        public PriorityStatus? Priority { get; set; }  // فلترة بالأولوية
        public WorkloadSort Sort { get; set; } = WorkloadSort.WorkloadDesc;
    }
}
