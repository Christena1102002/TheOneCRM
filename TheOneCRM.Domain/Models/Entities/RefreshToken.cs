using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.Entities
{
    public class RefreshToken :BaseEntity
    {
        public string TokenHash { get; set; } = string.Empty;

        public string OwnerId { get; set; } = string.Empty;
        public string? RemoteIpAddress { get; set; }
        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }

        //public string? RevokedByIpAddress { get; set; }
        public string? DeviceInfo { get; set; }
        public string? ReplacedByTokenHash { get; set; }

        public bool IsRememberMe { get; set; } = false;

        //public bool IsCompromised { get; set; } = false;

        //public DateTime? CompromisedAt { get; set; }

        //public string? CompromisedReason { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive =>
            !IsRevoked && !IsExpired;
        //public bool IsActive => !IsRevoked && !IsExpired && !IsCompromised;
    }
}
