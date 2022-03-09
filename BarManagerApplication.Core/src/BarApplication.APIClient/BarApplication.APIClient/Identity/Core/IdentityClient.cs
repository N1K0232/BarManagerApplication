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
            using var message = await httpClient.PostAsJsonAsync(Constants.LoginUrl, request);
            if (message.IsSuccessStatusCode)
            {
                var result = await message.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<AuthResponse>(result);
                return response;
            }
            else
            {
                return null;
            }
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