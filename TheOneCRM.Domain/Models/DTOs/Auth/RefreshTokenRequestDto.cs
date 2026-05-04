using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
