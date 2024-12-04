using Microsoft.AspNetCore.Identity;

namespace hippserver.Models.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; } = string.Empty;
    }
}