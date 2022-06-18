using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;

namespace BackendGestionaleBar.BusinessLayer.Services;

public interface IAuthenticationService
{
    Task<User> GetUserAsync(Guid id);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<RegisterResponse> RegisterClienteAsync(RegisterUserRequest request);
    Task<RegisterResponse> RegisterStaffAsync(RegisterUserRequest request);
    Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
}