using hippserver.Models.DTOs.Requests;
using hippserver.Models.DTOs.Responses;

namespace hippserver.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(string id);
        Task<UserResponse?> GetByEmailAsync(string email);
        Task<UserResponse?> GetByUsernameAsync(string username);
        Task<IEnumerable<UserResponse>> GetByRoleAsync(string roleName);
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest request);
        Task<bool> DeleteAsync(string id);
        Task<bool> ChangePasswordAsync(string id, ChangePasswordRequest request);
        Task<bool> AdminUpdatePasswordAsync(string userId, AdminUpdatePasswordRequest request);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUsernameUniqueAsync(string username);
    }
}