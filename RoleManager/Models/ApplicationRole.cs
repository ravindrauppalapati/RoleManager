using Microsoft.AspNetCore.Identity;

namespace RoleManager.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
