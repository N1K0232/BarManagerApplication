using System;
using System.Security.Claims;
using System.Security.Principal;

namespace BackendGestionaleBar.Authentication.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetId(this IPrincipal user)
            => Guid.Parse(GetClaimValue(user, ClaimTypes.NameIdentifier));

        public static string GetFirstName(this IPrincipal user)
            => GetClaimValue(user, ClaimTypes.GivenName);

        public static string GetLastName(this IPrincipal user)
            => GetClaimValue(user, ClaimTypes.Surname);

        public static string GetEmail(this IPrincipal user)
            => GetClaimValue(user, ClaimTypes.Email);

        public static string GetUserName(this IPrincipal user)
            => GetClaimValue(user, ClaimTypes.Name);

        public static DateTime GetBirthDate(this IPrincipal user)
            => DateTime.Parse(GetClaimValue(user, ClaimTypes.DateOfBirth));

        private static string GetClaimValue(IPrincipal user, string claimType)
            => ((ClaimsPrincipal)user).FindFirst(claimType)?.Value;
    }
}