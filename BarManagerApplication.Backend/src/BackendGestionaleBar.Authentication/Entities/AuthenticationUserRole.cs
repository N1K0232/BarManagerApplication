using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Entities;

public sealed class AuthenticationUserRole : IdentityUserRole<Guid>
{
    public AuthenticationUser User { get; set; }

    public AuthenticationRole Role { get; set; }
}