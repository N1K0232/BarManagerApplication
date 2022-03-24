using Microsoft.AspNetCore.Authorization;

namespace BackendGestionaleBar.DataAccessLayer.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public RoleAuthorizeAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}