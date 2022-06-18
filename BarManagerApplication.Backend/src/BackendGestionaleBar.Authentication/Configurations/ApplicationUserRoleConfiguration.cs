﻿using BackendGestionaleBar.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackendGestionaleBar.Authentication.Configurations;

internal class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId });

        builder.HasOne(userRole => userRole.User)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(userRole => userRole.UserId)
            .IsRequired();

        builder.HasOne(userRole => userRole.Role)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(userRole => userRole.RoleId)
            .IsRequired();
    }
}