using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings jwtSettings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public IdentityService(IOptions<JwtSettings> jwtSettingOptions, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            jwtSettings = jwtSettingOptions.Value;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var signInResult = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);
            if (!signInResult.Succeeded)
            {
                return null;
            }

            var user = await userManager.FindByNameAsync(request.UserName);
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.TelephoneNumber)
            }.Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var response = CreateToken(claims);

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExpirationMinutes);

            await userManager.UpdateAsync(user);

            return response;
        }
        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Email = request.Email,
                UserName = request.UserName
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                result = await userManager.AddToRoleAsync(user, RoleNames.Cliente);
            }

            var response = new RegisterResponse
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };

            return response;
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

                dbUser.RefreshToken = response.RefreshToken;
                var expirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExpirationMinutes);
                dbUser.RefreshTokenExpirationDate = expirationDate;

                await userManager.UpdateAsync(dbUser);

                return response;
            }

            return null;
        }
        public async Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new RegisterResponse
                {
                    Succeeded = false,
                    Errors = new List<string>
                    {
                        "Utente non esistente"
                    }
                };
            }

            var result = await userManager.ChangePasswordAsync(user, user.PasswordHash, request.NewPassword);
            return new RegisterResponse
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        private AuthResponse CreateToken(IEnumerable<Claim> claims)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jwtSettings.SecurityKey);
            SymmetricSecurityKey symmetricSecurityKey = new(bytes);
            SigningCredentials signInCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            DateTime notBefore = DateTime.UtcNow;
            DateTime expire = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes);

            JwtSecurityToken jwtSecurityToken = new(jwtSettings.Issuer, jwtSettings.Audience, claims,
                notBefore, expire, signInCredentials);

            JwtSecurityTokenHandler handler = new();

            string accessToken = handler.WriteToken(jwtSecurityToken);
            string refreshToken = GenerateRefreshToken();

            var response = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return response;
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[256];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
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
    }
}