using BackendGestionaleBar.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.Authentication.Configurations;

internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(user => user.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.DateOfBirth)
            .IsRequired();

        builder.Property(user => user.RefreshToken)
            .HasMaxLength(512)
            .IsRequired(false);

        builder.Property(user => user.RefreshTokenExpirationDate)
            .IsRequired(false);
    }
}