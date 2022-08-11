using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Contracts;

namespace BackendGestionaleBar.Internal;

internal class HttpUserService : IUserService
{
    private readonly HttpContext httpContext;

    public HttpUserService(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext;
    }

    public Guid GetId()
    {
        return httpContext.User.GetId();
    }

    public string GetUsername()
    {
        return httpContext.User.GetUserName();
    }
}