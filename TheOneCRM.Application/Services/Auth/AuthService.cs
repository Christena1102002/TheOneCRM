using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.DTOs.Auth;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.Auth;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
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
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "Email already in use"
                };
            }

            var user = _mapper.Map<AppUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = errors
                };
            }
            var addRole = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!addRole.Succeeded)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Message = "Cannot assign role to user"
                };
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

        public async Task<GenericResult<List<UsersDto>>> GetAllUsers()
        {
            var users = await _unitOfWork.Repository<AppUser>().ListAllAsync();
            if (users == null)
                return GenericResult<List<UsersDto>>.Failure("Not Found Any User");

            var results = users.Select(s => new UsersDto
            {
                Email = s.Email,
                Phone = s.PhoneNumber,
                UserId = s.Id,
                FullName = s.FullName
            }).ToList();
            return GenericResult<List<UsersDto>>.Success(results, "AllUsers retrieved successfully");
        }

        public async Task<GenericResult<UsersDto>> GetUsersByID(string userID)
        {
            //var user= await _unitOfWork.Users.GetByIdAsync(userID);
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null)
                return GenericResult<UsersDto>.Failure("Not Found this User");

            var results = new UsersDto
            {
                Email = user.Email,
                Phone = user.PhoneNumber,
                UserId = user.Id,
                FullName = user.FullName
            };
            return GenericResult<UsersDto>.Success(results, "Get User retrieved successfully");
        }

        public async Task<GenericResult<UsersDto>> UpdateUser(string userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return GenericResult<UsersDto>.Failure("Not Found this User");

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.Phone;
            user.Email = dto.Email;

            //_unitOfWork.Users.Update(user);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return GenericResult<UsersDto>.Failure("Failed to delete user");
            await _unitOfWork.SaveChangesAsync();

            var userDto = new UsersDto
            {
                UserId = user.Id,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FullName = user.FullName
            };
            return GenericResult<UsersDto>.Success(userDto, "Update this User Successfuly");
        }
        public async Task<Result> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure("Not Found this User");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return Result.Failure("Failed to delete user");
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Delete this User Successfuly");
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
    }
}
