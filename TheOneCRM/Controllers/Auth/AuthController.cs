using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TheOneCRM.API.Error;
using TheOneCRM.Application.DTOs.Auth;
using TheOneCRM.Application.Interfaces; 
using TheOneCRM.Domain.Models.DTOs.Auth;
namespace TheOneCRM.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login To Your Account")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var res = await _auth.LoginAsync(dto);
            return Ok(new ApiResponse(200, res.Message, res));
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Refresh Access Token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var res = await _auth.RefreshTokenAsync(dto.RefreshToken);
            return Ok(new ApiResponse(200, res.Message, res));
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new UnauthorizedAccessException("User not authenticated.");

            await _auth.LogoutAsync(request, userId);
            return Ok(new ApiResponse(200, "Logged out successfully"));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddApplicationUser")]
        [SwaggerOperation(Summary = "Create Accounts by Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var res = await _auth.RegisterAsync(dto);
            return Ok(new ApiResponse(200, res.Message, res));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        [SwaggerOperation(Summary = "GetAllUsersbyAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _auth.GetAllUsers();


            return Ok(new ApiResponse(200, "Users retrieved successfully", result));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserbyId")]
        [SwaggerOperation(Summary = " GetUserbyId(Admin)")]
        public async Task<IActionResult> GetUserById(string userid)
        {
            var result = await _auth.GetUsersByID(userid);
            return Ok(new ApiResponse(200, "User retrieved successfully", result));
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        [SwaggerOperation(Summary = " UpdateUserbyId(Admin)")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
        {
            var result = await _auth.UpdateUser(id, dto);
            return Ok(new ApiResponse(200, "User updated successfully", result));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        [SwaggerOperation(Summary = "DeleteUserbyId(Admin)")]
        public async Task<IActionResult> DeleteUser(string id)
        {
             await _auth.DeleteUser(id);

            return Ok(new { message = "User deleted successfully" });
        }
      
        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins()
        {
            var users = await _auth.GetUsersByRoleAsync("Admin");
            return Ok(new ApiResponse(200, "Success", users));
        }

    
        [HttpGet("developers")]
        public async Task<IActionResult> GetDevelopers()
        {
            var users = await _auth.GetUsersByRoleAsync("Developer");
            return Ok(new ApiResponse(200, "Success", users));
        }


        [HttpGet("sales")]
        public async Task<IActionResult> GetSales()
        {
            var users = await _auth.GetUsersByRoleAsync("Sales");
            return Ok(new ApiResponse(200, "Success", users));
        }

    
        [HttpGet("marketing")]
        public async Task<IActionResult> GetMarketing()
        {
            var users = await _auth.GetUsersByRoleAsync("Marketing");
            return Ok(new ApiResponse(200, "Success", users));
        }

        [HttpGet("support")]
        public async Task<IActionResult> GetSupport()
        {
            var users = await _auth.GetUsersByRoleAsync("Support");
            return Ok(new ApiResponse(200, "Success", users));
        }
    } 
}
