using BarApplication.APIClient.Identity.Models;
using BarApplication.APIClient.Identity.Models.Requests;
using BarApplication.APIClient.Identity.Models.Responses;
using System;
using System.Threading.Tasks;

namespace BarApplication.APIClient.Identity.Core
{
    public interface IIdentityClient : IDisposable
    {
        Task<User> GetUserAsync(string accessToken);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}