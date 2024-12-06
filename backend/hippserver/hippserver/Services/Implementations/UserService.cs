using Microsoft.AspNetCore.Identity;
using hippserver.Infrastructure.Repositories.Interfaces;
using hippserver.Models.Entities;
using hippserver.Models.DTOs.Requests;
using hippserver.Models.DTOs.Responses;
using hippserver.Services.Interfaces;

namespace hippserver.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

       
        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => MapToUserResponse(user));
        }

       
        public async Task<UserResponse?> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToUserResponse(user) : null;
        }

      
        public async Task<IEnumerable<UserResponse>> GetUsersByRoleAsync(string role)
        {
            var users = await _userRepository.GetAllUsersFromOneRoleAsync(role);
            return users.Select(user => MapToUserResponse(user));
        }

       
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
           
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                throw new InvalidOperationException($"Role '{request.Role}' does not exist.");
            }


           
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName ?? string.Empty,
                LastName = request.LastName ?? string.Empty,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create user: {errors}");
            }

           
            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign role '{request.Role}' to user: {errors}");
            }

            return MapToUserResponse(user);
        }

       
        public async Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.Email = request.Email ?? user.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to update user: {errors}");
            }

            return MapToUserResponse(user);
        }

        
        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            return deleteResult.Succeeded;
        }

       
        public async Task<bool> ChangePasswordAsync(string id, ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var changeResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            return changeResult.Succeeded;
        }

       

        // Map User to UserResponse
        private static UserResponse MapToUserResponse(ApplicationUser user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty
            };
        }
    }
}
