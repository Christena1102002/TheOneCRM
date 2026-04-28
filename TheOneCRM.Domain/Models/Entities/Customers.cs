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
    public class Customers :BaseEntity
    {
        public string FullName { set; get; }
        public string? Email { set; get; }
        public string? Phone { set; get; }
        public int Priority { set; get; }
        public StatusOfCustomers status { set;get; }
       public bool IsActiveCustomer { set; get; }

        //created-by
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser CreatedBy { get; set; }
        public string? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public AppUser AssignedTo { get; set; }

        public int compaignId { set; get; }
        public Campaigns campaigns { set; get; }


    }
}
