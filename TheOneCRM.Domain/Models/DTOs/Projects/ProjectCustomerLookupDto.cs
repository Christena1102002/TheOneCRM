namespace TheOneCRM.Domain.Models.DTOs.Projects
{
    // يرجّع اسم العميل واسم الشركة لما المدير يبعت الـ CustomerId
    public class ProjectCustomerLookupDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? CampanyName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
