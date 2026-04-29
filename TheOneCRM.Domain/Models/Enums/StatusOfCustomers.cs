using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Enums
{
    public enum StatusOfCustomers
    {
        none,
        Negotiating,   // جاري التفاوض
        Buyer,
        NotBuyer
        //Contacted, //تم التواصل معاه
        //Qualified, //موهل يكون عميل فعلي
        //Lost  // مش مهتم
    }
}
