namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerActivityDto
    {
        public int Id { get; set; }
        public string ActivityType { get; set; }
        public string? ContactResult { get; set; }
        public string? FromStatus { get; set; }
        public string? ToStatus { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
