using Microsoft.AspNetCore.Authorization;

namespace BackendGestionaleBar.Abstractions;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RoleAuthorizeAttribute : AuthorizeAttribute
{
	public RoleAuthorizeAttribute(params string[]? roles)
	{
		if (roles is null)
		{
			Roles = "Administrator";
		}
		else
		{
			Roles = string.Join(",", roles);
		}
	}
}