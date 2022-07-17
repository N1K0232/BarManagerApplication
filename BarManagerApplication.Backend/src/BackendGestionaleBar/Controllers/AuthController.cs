using BackendGestionaleBar.Abstractions;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Shared.Requests;
using BackendGestionaleBar.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

public class AuthController : ApiController
{
    private readonly IIdentityService identityService;

    public AuthController(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await identityService.LoginAsync(request);
        if (response != null)
        {
            return Ok(response);
        }

        return BadRequest("email o password errati");
    }

    [HttpPost("EnableTwoFactorAuthentication")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
        await identityService.EnableTwoFactorAuthenticationAsync();
        return NoContent();
    }

    [HttpPost("Refresh")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var response = await identityService.RefreshTokenAsync(request);
        if (response != null)
        {
            return Ok(response);
        }

        return BadRequest("errors during refreshing the token");
    }

    [HttpPost("RegisterCustomer")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterUserRequest request)
    {
        var response = await identityService.RegisterCustomerAsync(request);
        if (response.Succeeded)
        {
            return Ok("Utente registrato con successo");
        }

        return BadRequest(response);
    }

    [HttpPost("RegisterStaff")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterStaff([FromBody] RegisterUserRequest request)
    {
        var response = await identityService.RegisterStaffAsync(request);
        if (response.Succeeded)
        {
            return Ok("Utente registrato con successo");
        }

        return BadRequest(response);
    }

    [HttpPut("UpdatePassword")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {
        var response = await identityService.UpdatePasswordAsync(request);
        if (response.Succeeded)
        {
            return Ok("password cambiata con successo");
        }

        return BadRequest(response);
    }
}