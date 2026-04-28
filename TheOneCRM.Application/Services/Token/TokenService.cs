using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Data;

namespace TheOneCRM.Application.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;
        public TokenService(UserManager<AppUser> userManager, IConfiguration configuration, AppDbContext db)
        {
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
        }
        public async Task<(string accessToken, string refreshTokenPlain)> CreateTokenAsync(AppUser appUser, string ipAddress, bool rememberMe = false)
        {
            var claims = await BuildClaimsForAppUserAsync(appUser);
            var accessToken = GenerateJwtToken(claims);
            var (plainRefreshToken, refreshTokenEntity) = GenerateRefreshTokenEntity(ownerId: appUser.Id.ToString(),ipAddress: ipAddress,rememberMe: rememberMe);

            await _db.RefreshTokens.AddAsync(refreshTokenEntity);
            await _db.SaveChangesAsync();

            return (accessToken, plainRefreshToken);
        }

        public async Task<(string newAccessToken, string newRefreshTokenPlain)> RefreshTokenAsync(string refreshTokenPlain, string ipAddress)
        {
            var normalizedToken = NormalizeToken(refreshTokenPlain);
            if (string.IsNullOrWhiteSpace(normalizedToken))
                throw new SecurityTokenException("Invalid refresh token");

            var tokenHash = ComputeSha256Hash(normalizedToken);

            var existing = await _db.RefreshTokens
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

            if (existing == null)
                throw new SecurityTokenException("Invalid refresh token");

            

            if (existing.IsExpired)
                throw new SecurityTokenException("Refresh token expired");

           

            var (newPlainToken, newEntity) = GenerateRefreshTokenEntity(
                ownerId: existing.OwnerId,
                ipAddress: ipAddress,
                rememberMe: existing.IsRememberMe);

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;
            //existing.RevokedByIpAddress = ipAddress;
            existing.ReplacedByTokenHash = newEntity.TokenHash;

            _db.RefreshTokens.Update(existing);
            await _db.RefreshTokens.AddAsync(newEntity);

            var claims = await BuildClaimsFromRefreshTokenOwnerAsync(existing);
            var newAccessToken = GenerateJwtToken(claims);

            await _db.SaveChangesAsync();

            return (newAccessToken, newPlainToken);
        }
         private async Task<List<Claim>> BuildClaimsFromRefreshTokenOwnerAsync(RefreshToken token)
         { 
                var user = await _userManager.FindByIdAsync(token.OwnerId);
                if (user == null)
                    throw new SecurityTokenException("Invalid token owner");

                return await BuildClaimsForAppUserAsync(user);
         }
        private async Task<List<Claim>> BuildClaimsForAppUserAsync(AppUser appUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,appUser.Id.ToString()),
                new Claim(ClaimTypes.Email,appUser.Email?? string.Empty),
                  new Claim(ClaimTypes.Name,appUser.Email?? string.Empty),
            };
            var roles = await _userManager.GetRolesAsync(appUser);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
      
        private string GenerateJwtToken(List<Claim> claims)
        {
          
            var key = _configuration["JWT:Key"];
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("JWT Key missing");

            if (!double.TryParse(_configuration["JWT:AccessTokenDurationInMinutes"], out var minutes))
                throw new InvalidOperationException("Invalid JWT duration");

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(minutes),
            claims: claims,
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
             );
            return new JwtSecurityTokenHandler().WriteToken(token);

            
        }


        private (string plainToken, RefreshToken entity) GenerateRefreshTokenEntity(
          string ownerId,
          string ipAddress,
          bool rememberMe)
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            var plainToken = NormalizeToken(Convert.ToBase64String(bytes));
            var hash = ComputeSha256Hash(plainToken);

            var normal = _configuration["JWT:RefreshTokenDurationInDays"];
            var remember = _configuration["JWT:RefreshTokenRememberMeDurationInDays"];
            var selected = rememberMe ? remember : normal;

            if (!double.TryParse(selected, out var days))
                throw new InvalidOperationException("Invalid refresh duration");

            var entity = new RefreshToken
            {
                TokenHash = hash,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(days),
                RemoteIpAddress = ipAddress,
                IsRememberMe = rememberMe
            };

            return (plainToken, entity);
        }
        private static string NormalizeToken(string? token)
        {
            return token?.Trim().TrimEnd('=') ?? string.Empty;
        }
        private static string ComputeSha256Hash(string input)
        {
            var normalized = NormalizeToken(input);
            var bytes = Encoding.UTF8.GetBytes(normalized);
            var hash = SHA256.HashData(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
