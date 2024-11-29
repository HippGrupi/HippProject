using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using hippserver.Models.Entities;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace hippserver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<TestController> _logger;

        public TestController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<TestController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost("create-test-role")]
        public async Task<IActionResult> CreateTestRole()
        {
            try
            {
                var roleName = "TestRole";
                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    var role = new ApplicationRole
                    {
                        Name = roleName,
                        Description = "Test Role for Identity verification"
                    };
                    var result = await _roleManager.CreateAsync(role);

                    return Ok(new
                    {
                        success = result.Succeeded,
                        message = "Role created successfully",
                        errors = result.Errors
                    });
                }

                return Ok(new { message = "Role already exists" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("check-test-user")]
        public async Task<IActionResult> CheckTestUser()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync("testuser@test.com");
                if (user == null)
                {
                    return NotFound(new { message = "Test user not found" });
                }

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new
                {
                    userId = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    roles = roles,
                    hasPassword = await _userManager.HasPasswordAsync(user)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [HttpPost("create-test-user")]
        public async Task<IActionResult> CreateTestUser()
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync("testuser@test.com");
                if (existingUser != null)
                {
                    return BadRequest(new { message = "User already exists" });
                }

                var user = new ApplicationUser
                {
                    UserName = "testuser@test.com",
                    Email = "testuser@test.com",
                    FirstName = "Test",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "TestPass123!");

                if (result.Succeeded)
                {
                    // Ensure the role exists before adding
                    var roleExists = await _roleManager.RoleExistsAsync("TestRole");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole { Name = "TestRole" });
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, "TestRole");
                    return Ok(new
                    {
                        success = true,
                        message = "User created successfully",
                        userId = user.Id,
                        roleAdded = roleResult.Succeeded,
                        roleErrors = roleResult.Errors
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    errors = result.Errors.Select(e => new {
                        code = e.Code,
                        description = e.Description
                    })
                });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    errorMessage += $" | Inner Exception: {innerException.Message}";
                    innerException = innerException.InnerException;
                }

                return StatusCode(500, new
                {
                    error = errorMessage,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("verify-identity")]
        public async Task<IActionResult> VerifyIdentity()
        {
            try
            {
                var usersCount = await _userManager.Users.CountAsync();
                var rolesCount = await _roleManager.Roles.CountAsync();
                var testUser = await _userManager.FindByEmailAsync("testuser@test.com");
                var userRoles = testUser != null
                    ? await _userManager.GetRolesAsync(testUser)
                    : new List<string>();

                return Ok(new
                {
                    totalUsers = usersCount,
                    totalRoles = rolesCount,
                    testUserExists = testUser != null,
                    testUserRoles = userRoles
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}