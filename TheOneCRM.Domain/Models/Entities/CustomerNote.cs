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

        public string Note { get; set; }

        public string CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }

        public string Role { get; set; } // Marketing / Sales / Support
    }
}
