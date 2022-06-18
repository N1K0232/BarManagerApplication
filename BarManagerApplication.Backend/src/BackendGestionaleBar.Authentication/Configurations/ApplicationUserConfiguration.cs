using BackendGestionaleBar.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.Authentication.Configurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(user => user.FirstName).HasMaxLength(256).IsRequired();
        builder.Property(user => user.LastName).HasMaxLength(256).IsRequired();
        builder.Property(user => user.BirthDate).IsRequired();
    }
}