using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Contracts;
using System.Security.Claims;

namespace BackendGestionaleBar.Internal;

internal class InternalUserService : IUserService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public InternalUserService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => httpContextAccessor.HttpContext.User;

    public Guid GetId()
    {
        return User.Identity.IsAuthenticated ? User.GetId() : Guid.Empty;
    }

    public string GetUsername()
    {
        return User.Identity.IsAuthenticated ? User.GetUserName() : string.Empty;
    }
}