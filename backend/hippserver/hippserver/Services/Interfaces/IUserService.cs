using hippserver.Models.Entities;
using hippserver.Models.DTOs.Requests;
using hippserver.Models.DTOs.Responses;

namespace hippserver.Services.Interfaces
{
    public interface IUserService : IBaseService<ApplicationUser>
    {
        Task<UserResponse?> GetByEmailAsync(string email);
        Task<UserResponse?> GetByUsernameAsync(string username);
        Task<IEnumerable<UserResponse>> GetByRoleAsync(string roleName);
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest request);
        Task<bool> ChangePasswordAsync(string id, ChangePasswordRequest request);
        Task<bool> AdminUpdatePasswordAsync(string userId, AdminUpdatePasswordRequest request);
    }
}