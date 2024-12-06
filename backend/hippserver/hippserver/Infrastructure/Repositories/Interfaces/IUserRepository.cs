using hippserver.Models.Entities;

namespace hippserver.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {

        Task<IEnumerable<ApplicationUser>> GetAllUsersFromOneRoleAsync(string roleName);
        Task<ApplicationUser?> GetByIdAsync(string userId);

        Task<ApplicationUser?> GetByUsernameAsync(string username);
        Task AddUserAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task DeleteUserAsync(ApplicationUser user);
        Task UpdateUserAsync(ApplicationUser user);
    }
}