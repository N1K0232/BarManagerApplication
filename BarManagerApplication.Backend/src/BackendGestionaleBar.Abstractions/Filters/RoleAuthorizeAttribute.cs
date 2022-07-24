using Microsoft.AspNetCore.Authorization;

namespace BackendGestionaleBar.Abstractions.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}