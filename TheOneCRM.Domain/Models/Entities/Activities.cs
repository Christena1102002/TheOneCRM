using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;
namespace TheOneCRM.Domain.Models.Entities
{
    public class Activities:BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public TypeOfActivity Type { get; set; } 
            public string Description { get; set; }
            
         
       
    }
}
