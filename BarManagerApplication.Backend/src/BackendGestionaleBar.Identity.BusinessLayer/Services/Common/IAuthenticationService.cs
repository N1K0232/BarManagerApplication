﻿using BackendGestionaleBar.Shared.Requests;
using BackendGestionaleBar.Shared.Responses;

namespace BackendGestionaleBar.Identity.BusinessLayer.Services.Common;
public interface IAuthenticationService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<RegisterResponse> RegisterClienteAsync(RegisterUserRequest request);
    Task<RegisterResponse> RegisterStaffAsync(RegisterUserRequest request);
    Task<RegisterResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
}