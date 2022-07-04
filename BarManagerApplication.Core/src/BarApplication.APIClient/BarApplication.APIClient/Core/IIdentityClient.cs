using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;
using BackendGestionaleBar.Shared.Responses;
using System;
using System.Threading.Tasks;

namespace BarApplication.APIClient.Core;

public interface IIdentityClient : IDisposable
{
    Task<User> GetUserAsync(string accessToken);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<RegisterResponse> RegisterClienteAsync(RegisterUserRequest request);
    Task<RegisterResponse> RegisterStaffAsync(RegisterUserRequest request);
    Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
}