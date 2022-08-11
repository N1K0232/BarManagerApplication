using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Entities;

public sealed class AuthenticationUser : IdentityUser<Guid>
{
    public string Name { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? RefreshTokenExpirationDate { get; set; }

    public List<AuthenticationUserRole> UserRoles { get; set; }
}