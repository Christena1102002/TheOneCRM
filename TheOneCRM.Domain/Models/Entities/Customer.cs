using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    //العملاء المحتملين
    public class Customer :BaseEntity
    {
        public string FullName { set; get; }
        public string? Email { set; get; }
        public string? Phone { set; get; }
        public string CampanyName { set; get; }
        public PriorityStatus Priority { set; get; }
        public StatusOfCustomers status { set; get; } = StatusOfCustomers.New;
       //public bool IsActiveCustomer { set; get; }
       public string Notes {  set; get; }


        //created-by
        public string? CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }




        public string? AssignedToId { get; set; }
        public AppUser AssignedTo { get; set; }

        public int? compaignId { set; get; }
        public Campaigns? campaigns { set; get; }

        public ICollection<CustomerServices>? customerServices { get; set; } = new List<CustomerServices>();
    }
}
