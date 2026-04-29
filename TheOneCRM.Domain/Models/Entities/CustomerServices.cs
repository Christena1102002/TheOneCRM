using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class CustomerServices
    {
        public int customerId { get; set; }
        public Customer customers { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
