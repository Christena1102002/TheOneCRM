using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Application.DTOs.Auth
{
    public class AuthResultDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }

}
