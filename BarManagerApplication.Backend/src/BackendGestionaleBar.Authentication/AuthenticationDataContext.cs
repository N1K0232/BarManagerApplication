﻿using BackendGestionaleBar.Authentication.Entities;
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
    public AuthenticationDataContext(DbContextOptions<AuthenticationDataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}