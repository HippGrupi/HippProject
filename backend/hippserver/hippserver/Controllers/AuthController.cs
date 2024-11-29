using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using hippserver.Models.Entities;
using hippserver.Services.Interfaces;
using hippserver.Models.DTOs.Requests;
using hippserver.Models.DTOs.Responses;

namespace hippserver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return Unauthorized(new AuthResponse
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!passwordCheck)
                {
                    return Unauthorized(new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid password"
                    });
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user, roles);

                return Ok(new AuthResponse
                {
                    Success = true,
                    Token = token,
                    Username = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = $"Error during login: {ex.Message}"
                });
            }
        }
    }
}