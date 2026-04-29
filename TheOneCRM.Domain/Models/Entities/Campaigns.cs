using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Campaigns :BaseEntity
    {
        public string Name { get; set; }
        public ChannalSource Source { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AppUserId {  get; set; }
        public AppUser appUser { get; set; }
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
