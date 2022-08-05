using BackendGestionaleBar.Authentication.Entities;
using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authentication.Extensions;

public static class SignInManagerExtensions
{
    public static async Task<SignInResult> LoginAsync(this SignInManager<AuthenticationUser> signInManager, string email, string password)
    {
        AuthenticationUser user = await signInManager.UserManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        SignInResult signInResult = await signInManager.PasswordSignInAsync(user, password, false, false);
        return signInResult;
    }
}