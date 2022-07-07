using BackendGestionaleBar.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackendGestionaleBar.Authentication;

public class AuthenticationDataContext
    : IdentityDbContext<ApplicationUser,
      ApplicationRole,
      Guid,
      IdentityUserClaim<Guid>,
      ApplicationUserRole,
      IdentityUserLogin<Guid>,
      IdentityRoleClaim<Guid>,
      IdentityUserToken<Guid>>
{
    public AuthenticationDataContext(DbContextOptions<AuthenticationDataContext> options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}