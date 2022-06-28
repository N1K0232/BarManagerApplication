using Microsoft.AspNetCore.Authorization;

namespace BackendGestionaleBar.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}