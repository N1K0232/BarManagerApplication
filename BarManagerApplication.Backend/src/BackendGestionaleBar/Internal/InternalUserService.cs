using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Contracts;

namespace BackendGestionaleBar.Internal;

#nullable enable
internal class InternalUserService : IUserService
{
    private readonly HttpContext httpContext;

    public InternalUserService(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext!;
    }

    public Guid? GetId()
    {
        Guid userId = httpContext.User.GetId();
        if (userId == Guid.Empty)
        {
            return null;
        }
        return userId;
    }

    public string? GetUsername() => httpContext.User.GetUserName();
}
#nullable disable