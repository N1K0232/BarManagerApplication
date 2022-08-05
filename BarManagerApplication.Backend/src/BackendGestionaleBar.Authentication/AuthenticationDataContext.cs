using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackendGestionaleBar.Authentication;

public sealed class AuthenticationDataContext
    : IdentityDbContext<AuthenticationUser,
      AuthenticationRole,
      Guid,
      IdentityUserClaim<Guid>,
      AuthenticationUserRole,
      IdentityUserLogin<Guid>,
      IdentityRoleClaim<Guid>,
      IdentityUserToken<Guid>>
{
    public AuthenticationDataContext(DbContextOptions<AuthenticationDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        Assembly assembly = Assembly.GetExecutingAssembly();
        builder.ApplyConfigurationsFromAssembly(assembly);

        builder.ApplyTrimStringConverter();
    }
}