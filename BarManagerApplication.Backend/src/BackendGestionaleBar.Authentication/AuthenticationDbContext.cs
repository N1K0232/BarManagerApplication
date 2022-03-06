using BackendGestionaleBar.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendGestionaleBar.Authentication
{
    public class AuthenticationDbContext
        : IdentityDbContext<ApplicationUser,
          ApplicationRole,
          Guid,
          IdentityUserClaim<Guid>,
          ApplicationUserRole,
          IdentityUserLogin<Guid>,
          IdentityRoleClaim<Guid>,
          IdentityUserToken<Guid>>
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Type type = typeof(AuthenticationDbContext);
            builder.ApplyConfigurationsFromAssembly(type.Assembly);
            base.OnModelCreating(builder);
        }
    }
}