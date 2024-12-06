using Microsoft.EntityFrameworkCore;
using hippserver.Infrastructure.Data;
using hippserver.Infrastructure.Repositories.Interfaces;
using hippserver.Models.Entities;

namespace hippserver.Infrastructure.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

       
        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }

      
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersFromOneRoleAsync(string roleName)
        {
          
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return Enumerable.Empty<ApplicationUser>();

            var userRoleMappings = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .ToListAsync();

            var userIds = userRoleMappings.Select(ur => ur.UserId);
            return await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();
        }


        public async Task AddUserAsync(ApplicationUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

       
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

       
        public async Task DeleteUserAsync(ApplicationUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

       
        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

       
    }
}
