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
        [Authorize(Roles = "Admin")]
        [HttpPost("AddApplicationUser")]
        [SwaggerOperation(Summary = "Create Accounts by Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var res = await _auth.RegisterAsync(dto);

            if (!res.IsSuccess)
                return BadRequest(new ApiResponse(400, res.Message));

            return Ok(new ApiResponse(200, res.Message, res));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        [SwaggerOperation(Summary = "GetAllUsersbyAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _auth.GetAllUsers();
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.Message, result.Errors));

            return Ok(new ApiResponse(200, result.Message, result.Data));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserbyId")]
        [SwaggerOperation(Summary = " GetUserbyId(Admin)")]
        public async Task<IActionResult> GetUserById(string userid)
        {
            var result = await _auth.GetUsersByID(userid);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.Message, result.Errors));

            return Ok(new ApiResponse(200, result.Message, result.Data));
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        [SwaggerOperation(Summary = " UpdateUserbyId(Admin)")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
        {
            var result = await _auth.UpdateUser(id, dto);
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.Message, result.Errors));
            return Ok(new ApiResponse(200, result.Message, result.Data));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        [SwaggerOperation(Summary = "DeleteUserbyId(Admin)")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _auth.DeleteUser(id);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.Message, result.Errors));

            return Ok(new ApiResponse(200, result.Message));
        }
    } 
}
