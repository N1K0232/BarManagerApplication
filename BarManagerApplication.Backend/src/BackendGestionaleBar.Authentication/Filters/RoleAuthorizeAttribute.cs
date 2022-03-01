using Microsoft.AspNetCore.Authorization;

namespace BackendGestionaleBar.Authentication.Filters
{
    public sealed class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public RoleAuthorizeAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}