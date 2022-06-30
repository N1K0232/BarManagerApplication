using System.Security.Claims;
using System.Security.Principal;

namespace BackendGestionaleBar.Authentication.Extensions;

public static class ClaimsExtensions
{
    public static Guid GetId(this IPrincipal user)
    {
        string value = GetClaimValueInternal(user, ClaimTypes.NameIdentifier);
        return Guid.Parse(value);
    }

    public static string GetFirstName(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.GivenName);
    }

    public static string GetLastName(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Surname);
    }

    public static DateTime GetBirthDate(this IPrincipal user)
    {
        string value = GetClaimValueInternal(user, ClaimTypes.DateOfBirth);
        return DateTime.Parse(value);
    }

    public static string GetEmail(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Email);
    }

    public static string GetUserName(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.Name);
    }

    public static string GetPhoneNumber(this IPrincipal user)
    {
        return GetClaimValueInternal(user, ClaimTypes.MobilePhone);
    }

    public static string GetClaimValue(this IPrincipal user, string claimType)
    {
        return GetClaimValueInternal(user, claimType);
    }

    private static string GetClaimValueInternal(IPrincipal user, string claimType)
    {
        return ((ClaimsPrincipal)user).FindFirst(claimType)?.Value;
    }
}