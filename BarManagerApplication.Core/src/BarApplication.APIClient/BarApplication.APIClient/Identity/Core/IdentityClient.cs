using BarApplication.APIClient.Identity.Internal;
using BarApplication.APIClient.Identity.Models;
using BarApplication.APIClient.Identity.Models.Requests;
using BarApplication.APIClient.Identity.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BarApplication.APIClient.Identity.Core
{
    public sealed class IdentityClient : IIdentityClient
    {
        private readonly HttpClient httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<User> GetUserAsync(string accessToken)
        {
            using var client = new HttpClient();
            string authorization = $"{Constants.Bearer} {accessToken}";
            client.DefaultRequestHeaders.TryAddWithoutValidation(Constants.Authorization, authorization);

            var resource = new Uri(httpClient.BaseAddress, Constants.GetUserUrl);
            var user = await client.GetFromJsonAsync<User>(resource);
            return user;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            using var apiResponse = await httpClient.PostAsJsonAsync(Constants.LoginUrl, request);
            if (apiResponse.IsSuccessStatusCode)
            {
                var content = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<AuthResponse>(content);
                return response;
            }

            return null;
        }
        public async Task<RegisterResponse> RegisterClienteAsync(RegisterRequest request)
        {
            using var apiResponse = await httpClient.PostAsJsonAsync(Constants.RegisterClienteUrl, request);
            if (apiResponse.IsSuccessStatusCode)
            {
                var content = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<RegisterResponse>(content);
                return response;
            }

            return null;
        }
        public async Task<RegisterResponse> RegisterStaffAsync(RegisterRequest request)
        {
            using var apiResponse = await httpClient.PostAsJsonAsync(Constants.RegisterStaffUrl, request);
            if (apiResponse.IsSuccessStatusCode)
            {
                var content = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<RegisterResponse>(content);
                return response;
            }

            return null;
        }
        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            using var apiResponse = await httpClient.PostAsJsonAsync(Constants.RefreshTokenUrl, request);
            if (apiResponse.IsSuccessStatusCode)
            {
                var content = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<AuthResponse>(content);
                return response;
            }

            return null;
        }
        public async Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            using var apiResponse = await httpClient.PutAsJsonAsync(Constants.UpdatePasswordUrl, request);
            if (apiResponse.IsSuccessStatusCode)
            {
                var content = await apiResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<RegisterResponse>(content);
                return response;
            }

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing && httpClient != null)
            {
                httpClient.Dispose();
            }
        }
    }
}