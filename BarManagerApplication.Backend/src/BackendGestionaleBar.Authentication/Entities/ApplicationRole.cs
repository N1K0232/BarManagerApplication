using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Entities;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
    }
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public List<ApplicationUserRole> UserRoles { get; set; }
}