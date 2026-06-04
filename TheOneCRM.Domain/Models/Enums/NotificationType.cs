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

        [Description("موعد جديد")]
        AppointmentScheduled = 3,

        [Description("تحويل عميل للدعم")]
        CustomerTransferredToSupport = 4,

        [Description("رجوع عميل من الدعم")]
        CustomerReturnedToSales = 5,

        [Description("مهمة جديدة")]
        TaskAssigned = 6,

        [Description("رسالة من الإدارة")]
        AdminMessage = 7,

        [Description("عام")]
        General = 99
    }
}
