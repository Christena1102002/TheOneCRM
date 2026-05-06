using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TheOneCRM.Domain.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? imageUrl { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Address { get; set; }
        //public Department? Department { get; set; }
        //public ICollection<Activities> Activities { get; set; }
    }
}
