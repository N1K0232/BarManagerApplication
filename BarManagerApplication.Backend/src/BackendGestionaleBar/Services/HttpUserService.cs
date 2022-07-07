using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Contracts;
using System.Security.Claims;

namespace BackendGestionaleBar.Services;

public class HttpUserService : IUserService
{
	private readonly ClaimsPrincipal user;

	public HttpUserService(IHttpContextAccessor httpContextAccessor)
	{
		user = httpContextAccessor.HttpContext.User;
	}

	public Guid GetId()
	{
		return user.Identity.IsAuthenticated ? user.GetId() : Guid.Empty;
	}

	public string GetUsername()
	{
		return user.Identity.IsAuthenticated ? user.GetUserName() : string.Empty;
	}
}