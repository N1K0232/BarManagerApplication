﻿using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Entities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? RefreshTokenExpirationDate { get; set; }

    public List<ApplicationUserRole> UserRoles { get; set; }
}