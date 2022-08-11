using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Entities;

public sealed class AuthenticationRole : IdentityRole<Guid>
{
    public AuthenticationRole()
    {
    }
    public AuthenticationRole(string roleName) : base(roleName)
    {
    }

    public List<AuthenticationUserRole> UserRoles { get; set; }
}