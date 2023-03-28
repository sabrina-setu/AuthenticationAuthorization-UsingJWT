using Microsoft.AspNetCore.Identity;

namespace JWT.Core.Entities.Auth;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}


