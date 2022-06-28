using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Contracts;

namespace BackendGestionaleBar.Services;

public class HttpUserService : IUserService
{
	private readonly HttpContext httpContext;

	public HttpUserService(IHttpContextAccessor httpContextAccessor)
	{
		httpContext = httpContextAccessor.HttpContext;
	}

	public Guid GetId()
	{
		if (httpContext.User.Identity.IsAuthenticated)
		{
			return httpContext.User.GetId();
		}

		return Guid.Empty;
	}

	public string GetUsername()
	{
		if (httpContext.User.Identity.IsAuthenticated)
		{
			return httpContext.User.GetUserName();
		}

		return string.Empty;
	}
}