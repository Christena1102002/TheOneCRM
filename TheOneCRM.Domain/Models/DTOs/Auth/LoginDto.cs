using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.Auth
{
    public class LoginDto
    {
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
