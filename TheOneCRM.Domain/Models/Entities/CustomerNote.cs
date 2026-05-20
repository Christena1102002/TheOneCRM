using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class CustomerNote:BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
     
        public string? NoteMarketing { get; set; }
        public string? MarketingCreatedById { get; set; }
        public AppUser? MarketingCreatedBy { get; set; }
        public DateTime? MarketingCreatedAt { get; set; }



        public string? NoteSales { get; set; }
        public string? SalesCreatedById { get; set; }
        public AppUser? SalesCreatedBy { get; set; }
        public DateTime? SalesCreatedAt { get; set; }


        public string? NoteSupport { get; set; }
        public string? SupportCreatedById { get; set; }
        public AppUser? SupportCreatedBy { get; set; }
        public DateTime? SupportCreatedAt { get; set; }
        //public string CreatedById { get; set; }
        //public AppUser CreatedBy { get; set; }

        public string Role { get; set; } // Marketing / Sales / Support
    }
}
