using hippserver.Models.Entities;

namespace hippserver.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}