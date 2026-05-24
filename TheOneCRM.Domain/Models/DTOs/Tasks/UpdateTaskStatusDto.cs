using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Tasks
{
    // الـ developer بيغيّر حالة مهمته بس
    public class UpdateTaskStatusDto
    {
        public StatusOfTask Status { get; set; }

        // الساعات الفعلية (اختياري) — يسجّلها المطوّر لما يخلّص المهمة
        public int? ActualHours { get; set; }
    }
}
