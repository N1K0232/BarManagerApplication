using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BackendGestionaleBar.Authentication.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}