using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
    }
    public ApplicationRole(string roleName) : base(roleName)
    {
    }

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}