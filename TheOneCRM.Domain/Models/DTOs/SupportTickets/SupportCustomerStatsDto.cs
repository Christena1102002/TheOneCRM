namespace TheOneCRM.Domain.Models.DTOs.SupportTickets
{
    public class SupportCustomerStatsDto
    {
        public int TotalCustomers { get; set; }
        public int ConsultedCustomers { get; set; }
        public int WaitingConsultation { get; set; }
        public int CustomersWithNotes { get; set; }
    }
}
