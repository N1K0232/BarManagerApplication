using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public interface IIdentityService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task<RegisterResponse> RegisterClienteAsync(RegisterRequest request);
        Task<RegisterResponse> RegisterStaffAsync(RegisterRequest request);
        Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
    }
}