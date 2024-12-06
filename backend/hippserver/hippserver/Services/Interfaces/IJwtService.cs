using hippserver.Models.Entities;
using System.Security.Claims;

namespace hippserver.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
        ClaimsPrincipal ValidateToken(string token);
    }
}