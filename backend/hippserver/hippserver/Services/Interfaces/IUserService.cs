using hippserver.Models.Entities;
using hippserver.Models.DTOs.Requests;
using hippserver.Models.DTOs.Responses;

namespace hippserver.Services.Interfaces
{
    public interface IUserService 
    {
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(string id);
        
        Task<IEnumerable<UserResponse>> GetUsersByRoleAsync(string role);
        
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(string id);

        Task<bool> ChangePasswordAsync(string id, ChangePasswordRequest request);
       
    }
}