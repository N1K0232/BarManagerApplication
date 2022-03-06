using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
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
                new Claim(ClaimTypes.Email, user.Email)
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

        private AuthResponse CreateToken(IEnumerable<Claim> claims)
        {
            var bytes = Encoding.UTF8.GetBytes(jwtSettings.SecurityKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(bytes);
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var notBefore = DateTime.UtcNow;
            var expire = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes);

            var jwtSecurityToken = new JwtSecurityToken(jwtSettings.Issuer, jwtSettings.Audience, claims,
                notBefore, expire, signInCredentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var response = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken()
            };

            return response;

            static string GenerateRefreshToken()
            {
                var randomNumber = new byte[256];
                using var generator = RandomNumberGenerator.Create();
                generator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}