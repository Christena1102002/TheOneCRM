using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Application.Common;
using TheOneCRM.Application.DTOs.Auth;
using TheOneCRM.Domain.Models.DTOs.Auth;

namespace TheOneCRM.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto dto);
        Task<AuthResultDto?> LoginAsync(LoginDto dto);
        Task<GenericResult<List<UsersDto>>> GetAllUsers();
        Task<GenericResult<UsersDto>> GetUsersByID(string userID);
        Task<GenericResult<UsersDto>> UpdateUser(string userId, UpdateUserDto dto);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
        Task<Result> DeleteUser(string userId);
    }
}
