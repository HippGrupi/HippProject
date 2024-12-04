using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using hippserver.Models.Entities;
using hippserver.Models.DTOs.Requests;
using hippserver.Models.DTOs.Responses;
using hippserver.Models.Enums;


namespace hippserver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.Admin)]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => new RoleResponse
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            });
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponse>> GetRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(new RoleResponse
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty
            });
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignUserToRole([FromBody] AdminRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            if (await _userManager.IsInRoleAsync(user, role.Name))
                return BadRequest(new { message = "User is already in this role" });

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
                return BadRequest(new { message = "Failed to assign role to user" });

            return Ok(new { message = "Role assigned successfully" });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] AdminRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            if (!await _userManager.IsInRoleAsync(user, role.Name))
                return BadRequest(new { message = "User is not in this role" });

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!result.Succeeded)
                return BadRequest(new { message = "Failed to remove role from user" });

            return Ok(new { message = "Role removed successfully" });
        }
    }
}