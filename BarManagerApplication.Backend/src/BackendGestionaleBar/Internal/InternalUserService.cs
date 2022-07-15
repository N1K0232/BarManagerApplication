using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Contracts;

namespace BackendGestionaleBar.Internal;

internal class InternalUserService : IUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public InternalUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid GetId()
    {
        return httpContextAccessor.HttpContext.User.GetId();
    }

    public string GetUsername()
    {
        return httpContextAccessor.HttpContext.User.GetUserName();
    }
}