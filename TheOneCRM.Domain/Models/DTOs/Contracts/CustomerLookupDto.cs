using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.Contracts
{
    public class CustomerLookupDto
    {
        public int Id { get; set; }
        public string FullName { set; get; }
        public string? Phone { set; get; }
        public string CampanyName { set; get; }
        public string? Address { get; set; }
    }
}
