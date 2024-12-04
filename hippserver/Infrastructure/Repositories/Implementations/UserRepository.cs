using Microsoft.EntityFrameworkCore;
using hippserver.Infrastructure.Data;
using hippserver.Infrastructure.Repositories.Interfaces;
using hippserver.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace hippserver.Infrastructure.Repositories.Implementations
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IEnumerable<ApplicationUser>> GetByRoleAsync(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            return user == null;
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            var user = await GetByUsernameAsync(username);
            return user == null;
        }
    }
}