using System;
using System.Security.Claims;
using System.Security.Principal;

namespace BackendGestionaleBar.Authentication.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetId(this IPrincipal user)
        {
            string value = GetClaimValue(user, ClaimTypes.NameIdentifier);
            return Guid.Parse(value);
        }

        private static string GetClaimValue(IPrincipal user, string claimType)
        {
            string value = ((ClaimsPrincipal)user).FindFirst(claimType)?.Value;
            return value;
        }
    }
}