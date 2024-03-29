﻿using BackendGestionaleBar.Shared.Requests;
using BackendGestionaleBar.Shared.Responses;

namespace BackendGestionaleBar.BusinessLayer.Services.Interfaces;

public interface IIdentityService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task EnableTwoFactorAuthenticationAsync();
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<RegisterResponse> RegisterCustomerAsync(RegisterUserRequest request);
    Task<RegisterResponse> RegisterStaffAsync(RegisterUserRequest request);
    Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
}