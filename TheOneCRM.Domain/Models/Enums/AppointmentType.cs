using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Enums
{
    public enum AppointmentType
    {
        Meeting = 1,            // اجتماع
        Demo = 2,               // عرض تجريبي
        Call = 3,               // مكالمة
        FollowUp = 4,           // متابعة
        Presentation = 5,       // عرض تقديمي
        Negotiation = 6,        // تفاوض
        ContractSigning = 7,    // توقيع عقد
        Support = 8,            // دعم فني
        Other = 9
    }
}
