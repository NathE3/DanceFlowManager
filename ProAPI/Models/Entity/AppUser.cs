using Microsoft.AspNetCore.Identity;

namespace RestAPI.Models.Entity
{
    public abstract class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
