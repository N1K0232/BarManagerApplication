﻿using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.Shared.Requests;
using BackendGestionaleBar.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly RandomNumberGenerator generator;

    private readonly JwtSettings jwtSettings;

    private readonly UserManager<AuthenticationUser> userManager;
    private readonly SignInManager<AuthenticationUser> signInManager;

    public IdentityService(IOptions<JwtSettings> jwtSettingsOptions, UserManager<AuthenticationUser> userManager, SignInManager<AuthenticationUser> signInManager)
    {
        generator = RandomNumberGenerator.Create();
        jwtSettings = jwtSettingsOptions.Value;

        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var signInResult = await signInManager.LoginAsync(request.Email, request.Password);
        if (signInResult == null || !signInResult.Succeeded)
        {
            return null;
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
        }.Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var response = CreateToken(claims);
        await SaveRefreshTokenAsync(user, response.RefreshToken);
        return response;
    }
    public Task EnableTwoFactorAuthenticationAsync() => Task.CompletedTask;
    public async Task<RegisterResponse> RegisterCustomerAsync(RegisterUserRequest request)
    {
        var user = new AuthenticationUser
        {
            Name = $"{request.FirstName} {request.LastName}",
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            UserName = request.UserName
        };

        var result = await userManager.RegisterAsync(user, request.Password, RoleNames.Customer);
        return new(result.Succeeded, result.Errors.Select(e => e.Description));
    }
    public async Task<RegisterResponse> RegisterStaffAsync(RegisterUserRequest request)
    {
        var user = new AuthenticationUser
        {
            Name = $"{request.FirstName} {request.LastName}",
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            UserName = request.UserName
        };

        var result = await userManager.RegisterAsync(user, request.Password, RoleNames.Staff);
        return new(result.Succeeded, result.Errors.Select(e => e.Description));
    }
    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = ValidateAccessToken(request.AccessToken);
        if (user != null)
        {
            var userId = user.GetId();
            var dbUser = await userManager.FindByIdAsync(userId.ToString());

            if (dbUser?.RefreshToken == null || dbUser?.RefreshTokenExpirationDate < DateTime.UtcNow || dbUser?.RefreshToken != request.RefreshToken)
            {
                return null;
            }

            var response = CreateToken(user.Claims);
            await SaveRefreshTokenAsync(dbUser, response.RefreshToken);
            return response;
        }

        return null;
    }
    public async Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new(false, new List<string> { "User not found" });
        }

        var result = await userManager.ChangePasswordAsync(user, user.PasswordHash, request.NewPassword);
        return new(result.Succeeded, result.Errors.Select(e => e.Description));
    }

    private AuthResponse CreateToken(IEnumerable<Claim> claims)
    {
        string accessToken = GetAccessToken(claims);
        string refreshToken = GetRefreshToken();
        return new(accessToken, refreshToken);
    }
    private string GetAccessToken(IEnumerable<Claim> claims)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(jwtSettings.SecurityKey);
        SymmetricSecurityKey symmetricSecurityKey = new(bytes);
        SigningCredentials signInCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        DateTime notBefore = DateTime.UtcNow;
        DateTime expire = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes);

        JwtSecurityToken jwtSecurityToken = new(jwtSettings.Issuer, jwtSettings.Audience, claims,
            notBefore, expire, signInCredentials);

        JwtSecurityTokenHandler handler = new();
        return handler.WriteToken(jwtSecurityToken);
    }
    private string GetRefreshToken()
    {
        byte[] randomNumber = new byte[256];
        generator.GetBytes(randomNumber);
        generator.Dispose();
        return Convert.ToBase64String(randomNumber);
    }
    private ClaimsPrincipal ValidateAccessToken(string accessToken)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var user = tokenHandler.ValidateToken(accessToken, parameters, out var securityToken);
            if (securityToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg == SecurityAlgorithms.HmacSha256)
            {
                return user;
            }
        }
        catch
        {
        }

        return null;
    }
    private async Task SaveRefreshTokenAsync(AuthenticationUser user, string refreshToken)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExpirationMinutes);
        await userManager.UpdateAsync(user);
    }
}