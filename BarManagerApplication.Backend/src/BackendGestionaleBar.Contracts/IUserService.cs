using System.Security.Claims;

namespace BackendGestionaleBar.Contracts;

public interface IUserService
{
    ClaimsPrincipal User { get; }

    Guid GetId();
    string GetUsername();
}