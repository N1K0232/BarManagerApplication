using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BackendGestionaleBar.Authentication.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDate { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}