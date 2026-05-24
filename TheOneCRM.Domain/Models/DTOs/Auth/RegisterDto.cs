using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Application.DTOs.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        // تخصّص المطوّر (اختياري) — يتحدّد للمطورين فقط
        public string? Specialty { get; set; }
    }
}
