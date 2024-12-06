using hippserver.Models.DTOs.Responses;
using hippserver.Models.DTOs.Requests;
using hippserver.Models.Entities;
using hippserver.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using hippserver.Services.Implementations;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IUserService _userService;

    public AuthController(UserManager<ApplicationUser> userManager, IJwtService jwtService, IUserService userService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _userService = userService;
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

    [HttpPost("validate")]
    public IActionResult ValidateToken([FromBody] string token)
    {
        var principal = _jwtService.ValidateToken(token);

        if (principal == null)
        {
            return Unauthorized(new { Success = false, Message = "Invalid or expired token" });
        }

        return Ok(new { Success = true, Message = "Token is valid" });
    }
    // Duhet me ndreq
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] CreateUserRequest request)
    {
        try
        {
            // Ensure the required fields are provided
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Role))
            {
                return BadRequest(new { Success = false, Message = "Username, password, and role are required." });
            }

            // Call the service to register the user
            await _userService.CreateUserAsync(request);

            return Ok(new { Success = true, Message = $"User '{request.Username}' registered successfully with role '{request.Role}'." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Success = false, Message = ex.Message });
        }
    }
}
