namespace TheOneCRM.Domain.Models.DTOs.Tasks
{
    public class TaskAssigneeDto
    {
        public string UserId { get; set; } = null!;
        public string? FullName { get; set; }
    }
}
