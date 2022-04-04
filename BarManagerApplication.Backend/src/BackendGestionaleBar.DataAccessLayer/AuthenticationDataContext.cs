using BackendGestionaleBar.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendGestionaleBar.DataAccessLayer
{
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
        public AuthenticationDataContext(DbContextOptions<AuthenticationDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(user =>
            {
                user.Property(user => user.FirstName).HasMaxLength(256).IsRequired();
                user.Property(user => user.LastName).HasMaxLength(256).IsRequired();
                user.Property(user => user.BirthDate).IsRequired();
            });

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
        }
    }
}