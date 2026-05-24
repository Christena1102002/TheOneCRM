using System.ComponentModel.DataAnnotations.Schema;

namespace TheOneCRM.Domain.Models.Entities
{
    // جدول الربط بين المشروع والمهندسين المعيّنين له
    public class ProjectEngineer
    {
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Projects Project { get; set; } = null!;

        public string EngineerId { get; set; } = null!;

        [ForeignKey("EngineerId")]
        public AppUser Engineer { get; set; } = null!;
    }
}
