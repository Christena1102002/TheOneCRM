using System.ComponentModel;

namespace TheOneCRM.Domain.Models.Enums
{
    public enum NotificationType
    {
        [Description("عميل جديد")]
        NewCustomerAssigned = 0,

        [Description("تذكرة دعم جديدة")]
        NewSupportTicket = 1,

        [Description("متابعة قادمة")]
        UpcomingFollowUp = 2,

        [Description("عام")]
        General = 99
    }
}
