using BackendGestionaleBar.Authentication.Entities;
using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Extensions;

public static class UserManagerExtensions
{
    public static async Task<IdentityResult> RegisterAsync(this UserManager<AuthenticationUser> userManager, AuthenticationUser user, string password, string role)
    {
        IdentityResult identityResult = await TryRegisterAsync(userManager, user, password);
        if (identityResult.Succeeded)
        {
            identityResult = await userManager.AddToRoleAsync(user, role);
        }

        return identityResult;
    }

    internal static async Task<IdentityResult> RegisterAsync(this UserManager<AuthenticationUser> userManager, AuthenticationUser user, string password, params string[] roles)
    {
        IdentityResult identityResult = await TryRegisterAsync(userManager, user, password);
        if (identityResult.Succeeded)
        {
            identityResult = await userManager.AddToRolesAsync(user, roles);
        }

        return identityResult;
    }

    private static async Task<IdentityResult> TryRegisterAsync(UserManager<AuthenticationUser> userManager, AuthenticationUser user, string password)
    {
        AuthenticationUser dbUser = await userManager.FindByEmailAsync(user.Email) ?? await userManager.FindByNameAsync(user.UserName);
        if (dbUser != null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "a user with the same username or email already exists" });
        }

        IdentityResult identityResult = await userManager.CreateAsync(user, password);
        return identityResult;
    }
}