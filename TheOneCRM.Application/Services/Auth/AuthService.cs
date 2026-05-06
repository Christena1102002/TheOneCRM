using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.DTOs.Auth;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.Auth;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheOneCRM.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, UserManager<AppUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<AuthResultDto?> LoginAsync(LoginDto dto)
        {
            var User = await _userManager.FindByEmailAsync(dto.Email);
            if (User == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (await _userManager.IsLockedOutAsync(User))
                throw new InvalidOperationException("Account is locked. Please try again later.");


            var isPasswordValid = await _userManager.CheckPasswordAsync(User, dto.Password);
            if (!isPasswordValid)
            {
                await _userManager.AccessFailedAsync(User); // عشان الـ lockout counter
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            await _userManager.ResetAccessFailedCountAsync(User);
            var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(User);
            var roles = await _userManager.GetRolesAsync(User);
            var role = roles.FirstOrDefault();
            var result = new AuthResultDto
            {
                IsSuccess = true,
                Message = "Login Successfully",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Role = role,
                UserId = User.Id,
                Email = User.Email
            };
            return result;
        }
        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
            {
                throw new InvalidOperationException("Email already in use");
            }

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException("Email already in use");
            }

            var addRole = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!addRole.Succeeded)
            {
                throw new InvalidOperationException("Cannot assign role to user");
            }
            var (accessToken, refreshToken) = await _tokenService.CreateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            return new AuthResultDto
            {
                IsSuccess = true,
                UserId = user.Id,
                Email = user.Email,
                Role = role,
                AccessToken=accessToken,
                RefreshToken=refreshToken,
                //Message = "Account create Successfuly"
            };
        }

        public async Task<List<UsersDto>> GetAllUsers()
        {
            var users = await _unitOfWork.Repository<AppUser>().ListAllAsync();
                if (users == null || !users.Any())
                    throw new KeyNotFoundException("No users found");
            var result = new List<UsersDto>();
            foreach(var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                result.Add(new UsersDto
                {

                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    UserId = user.Id,
                    FullName = user.FullName,
                    Role = role.FirstOrDefault(),
                    Address = user.Address


                });
            }
            return result;
        }

        public async Task<UsersDto> GetUsersByID(string userID)
        {
            //var user= await _unitOfWork.Users.GetByIdAsync(userID);
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
                throw new KeyNotFoundException($"User with id '{userID}' not found");
            var role = await _userManager.GetRolesAsync(user);
            return new UsersDto
            {
                Email = user.Email,
                Phone = user.PhoneNumber,
                UserId = user.Id,
                FullName = user.FullName,
                Address=user.Address,
                Role=role.FirstOrDefault()

            };
            
        }

        public async Task<UsersDto> UpdateUser(string userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with id '{userId}' not found");


            user.FullName = dto.FullName;
            user.PhoneNumber = dto.Phone;
            user.Email = dto.Email;
            user.Address = dto.Address;
            user.UserName = dto.Email;

            //_unitOfWork.Users.Update(user);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update user: {errors}");
            }

            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                var roleExists = await _roleManager.RoleExistsAsync(dto.Role);
                if (!roleExists)
                    throw new KeyNotFoundException($"Role '{dto.Role}' does not exist.");


                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Failed to remove old roles: {errors}");
                    }
                }
                var addResult = await _userManager.AddToRoleAsync(user, dto.Role);
                if (!addResult.Succeeded)
                {
                    var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Failed to assign new role: {errors}");
                }
            
            }
            var roles = await _userManager.GetRolesAsync(user);
           
           
        

            await _unitOfWork.SaveChangesAsync();

            return new UsersDto
            {
                UserId = user.Id,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FullName = user.FullName,
                Address = user.Address,
                Role= roles.FirstOrDefault()
            };
        }
        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with id '{userId}' not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to delete user: {errors}");
            }
            await _unitOfWork.SaveChangesAsync();
           
        }
        public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new InvalidOperationException("Refresh token is required");

            var (newAccessToken, newRefreshTokenPlain,user) = await _tokenService
                .RefreshTokenAsync(refreshToken);
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthResultDto
            {
                IsSuccess = true,
                Message = "Token refreshed successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenPlain,
                Role=roles.FirstOrDefault(),
                UserId=user.Id.ToString(),
                Email=user.Email
            };
        }
        public async Task LogoutAsync(LogoutRequestDto request, string userId)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new InvalidOperationException("Refresh token is required");
            var tokenHash= _tokenService.HashRefreshToken(request.RefreshToken);

            var spec = new RefreshTokenByHashSpec(tokenHash);
            var refreshToken = await _unitOfWork
                .Repository<RefreshToken>()
                .GetEntityWithSpec(spec);

            if (refreshToken is null)
                throw new KeyNotFoundException("Refresh token not found.");

            if (refreshToken.OwnerId != userId)
                throw new InvalidOperationException("Token does not belong to current user.");

            if (refreshToken.IsRevoked)
                throw new InvalidOperationException("Refresh token already revoked.");

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;

            _unitOfWork.Repository<RefreshToken>().Update(refreshToken);
            await _unitOfWork.SaveChangesAsync();


        }

        public async Task<List<UsersDto>> GetUsersByRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);

            var result = new List<UsersDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UsersDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    FullName = user.FullName,
                    Role = roles.FirstOrDefault(),
                    Address=user.Address
                });
            }

            return result;
        }
    }
}
