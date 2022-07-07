using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;
using BackendGestionaleBar.Shared.Responses;
using BarApplication.APIClient.Internal;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BarApplication.APIClient.Core;

public sealed class IdentityClient : IIdentityClient
{
    private bool _disposed = false;

    private readonly HttpClient _httpClient;

    public IdentityClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    ~IdentityClient()
    {
        Dispose(false);
    }

    public async Task<User> GetUserAsync(string accessToken)
    {
        ThrowIfDisposed();

        using var client = new HttpClient();
        string authorization = $"{Constants.Bearer} {accessToken}";
        client.DefaultRequestHeaders.TryAddWithoutValidation(Constants.Authorization, authorization);

        var resource = new Uri(new Uri("https://localhost:44388/"), Constants.GetUserUrl);
        var user = await client.GetFromJsonAsync<User>(resource);
        return user;
    }
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        ThrowIfDisposed();

        using var apiResponse = await _httpClient.PostAsJsonAsync(Constants.LoginUrl, request);
        if (apiResponse.IsSuccessStatusCode)
        {
            var content = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<AuthResponse>(content);
            return response;
        }

        return null;
    }
    public async Task<RegisterResponse> RegisterClienteAsync(RegisterUserRequest request)
    {
        ThrowIfDisposed();

        using var apiResponse = await _httpClient.PostAsJsonAsync(Constants.RegisterClienteUrl, request);
        if (apiResponse.IsSuccessStatusCode)
        {
            var content = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RegisterResponse>(content);
            return response;
        }

        return null;
    }
    public async Task<RegisterResponse> RegisterStaffAsync(RegisterUserRequest request)
    {
        ThrowIfDisposed();

        using var apiResponse = await _httpClient.PostAsJsonAsync(Constants.RegisterStaffUrl, request);
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
        ThrowIfDisposed();

        using var apiResponse = await _httpClient.PostAsJsonAsync(Constants.RefreshTokenUrl, request);
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
        ThrowIfDisposed();

        using var apiResponse = await _httpClient.PutAsJsonAsync(Constants.UpdatePasswordUrl, request);
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
        if (disposing && !_disposed)
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
            _disposed = true;
        }
    }
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}