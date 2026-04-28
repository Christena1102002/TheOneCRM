using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Domain.Interfaces
{
    public interface ITokenService
    {
        //Task<string> GenerateToken(AppUser user);
        Task<(string accessToken, string refreshTokenPlain)> CreateTokenAsync(AppUser appUser, string ipAddress, bool rememberMe = false);
        Task<(string newAccessToken, string newRefreshTokenPlain)> RefreshTokenAsync(string refreshTokenPlain, string ipAddress);

    }
}
