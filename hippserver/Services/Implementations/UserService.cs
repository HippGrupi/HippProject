using Microsoft.AspNetCore.Identity;
using hippserver.Infrastructure.Repositories.Interfaces;
using hippserver.Models.Entities;
using hippserver.Models.DTOs.Requests;
using hippserver.Models.DTOs.Responses;
using hippserver.Services.Interfaces;
using hippserver.Models.Enums;

namespace hippserver.Services.Implementations
{
    public class UserService : BaseService<ApplicationUser>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            ILogger<UserService> logger) : base(userRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var responses = new List<UserResponse>();

            foreach (var user in users)
            {
                responses.Add(await MapToUserResponse(user));
            }

            return responses;
        }

        public async Task<UserResponse?> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? await MapToUserResponse(user) : null;
        }

        public async Task<UserResponse?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? await MapToUserResponse(user) : null;
        }

        public async Task<UserResponse?> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user != null ? await MapToUserResponse(user) : null;
        }

        public async Task<IEnumerable<UserResponse>> GetByRoleAsync(string roleName)
        {
            var users = await _userRepository.GetByRoleAsync(roleName);
            var responses = new List<UserResponse>();

            foreach (var user in users)
            {
                responses.Add(await MapToUserResponse(user));
            }

            return responses;
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            _logger.LogInformation("Creating new user with username: {Username}", request.Username);

            // Validate roles first
            if (request.Roles != null && request.Roles.Any())
            {
                var invalidRoles = request.Roles.Where(role => !UserRoles.IsValidRole(role)).ToList();
                if (invalidRoles.Any())
                {
                    var errorMessage = $"Invalid role(s) specified: {string.Join(", ", invalidRoles)}. " +
                                     $"Valid roles are: {string.Join(", ", UserRoles.AllRoles)}";
                    _logger.LogWarning("Invalid roles requested: {InvalidRoles}", string.Join(", ", invalidRoles));
                    throw new InvalidOperationException(errorMessage);
                }
            }

            // Validate email and username
            if (await _userRepository.IsEmailUniqueAsync(request.Email) == false)
            {
                _logger.LogWarning("Email already in use: {Email}", request.Email);
                throw new InvalidOperationException("Email is already in use");
            }

            if (await _userRepository.IsUsernameUniqueAsync(request.Username) == false)
            {
                _logger.LogWarning("Username already in use: {Username}", request.Username);
                throw new InvalidOperationException("Username is already in use");
            }

            // Create user
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName ?? string.Empty,
                LastName = request.LastName ?? string.Empty,
                EmailConfirmed = true
            };

            // Create the user with password
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user. Errors: {Errors}", errors);
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

            // Assign roles if any are specified
            if (request.Roles != null && request.Roles.Any())
            {
                _logger.LogInformation("Assigning roles to user: {Roles}", string.Join(", ", request.Roles));

                foreach (var role in request.Roles)
                {
                    result = await _userManager.AddToRoleAsync(user, role);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        _logger.LogError("Failed to assign role {Role}. Errors: {Errors}", role, errors);
                        throw new InvalidOperationException($"Failed to assign role {role}: {errors}");
                    }
                }
            }

            _logger.LogInformation("Successfully created user with ID: {UserId}", user.Id);
            return await MapToUserResponse(user);
        }

        public async Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest request)
        {
            _logger.LogInformation("Updating user with ID: {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                throw new InvalidOperationException("User not found");
            }

            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Email = request.Email ?? user.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update user. Errors: {Errors}", errors);
                throw new InvalidOperationException($"Failed to update user: {errors}");
            }

            _logger.LogInformation("Successfully updated user with ID: {UserId}", id);
            return await MapToUserResponse(user);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to delete user. Errors: {Errors}", errors);
                return false;
            }

            _logger.LogInformation("Successfully deleted user with ID: {UserId}", id);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string id, ChangePasswordRequest request)
        {
            _logger.LogInformation("Changing password for user with ID: {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", id);
                throw new InvalidOperationException("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to change password. Errors: {Errors}", errors);
            }

            return result.Succeeded;
        }

        public async Task<bool> AdminUpdatePasswordAsync(string userId, AdminUpdatePasswordRequest request)
        {
            _logger.LogInformation("Admin updating password for user with ID: {UserId}", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                throw new InvalidOperationException("User not found");
            }

            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update password. Errors: {Errors}", errors);
                throw new InvalidOperationException($"Failed to set new password: {errors}");
            }

            _logger.LogInformation("Successfully updated password for user with ID: {UserId}", userId);
            return true;
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _userRepository.IsEmailUniqueAsync(email);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return await _userRepository.IsUsernameUniqueAsync(username);
        }

        private async Task<UserResponse> MapToUserResponse(ApplicationUser user)
        {
            if (user == null) return null!;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserResponse
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Roles = roles.ToList()
            };
        }
    }
}