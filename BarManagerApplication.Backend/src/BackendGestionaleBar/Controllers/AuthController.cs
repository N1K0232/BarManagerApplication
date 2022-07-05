using BackendGestionaleBar.BusinessLayer.Services.Common;
using BackendGestionaleBar.Shared.Requests;
using BackendGestionaleBar.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService authenticationService;

    public AuthController(IIdentityService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authenticationService.LoginAsync(request);
        if (response != null)
        {
            return Ok(response);
        }

        return BadRequest("email o password errati");
    }

    [HttpPost("Refresh")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var response = await authenticationService.RefreshTokenAsync(request);
        if (response != null)
        {
            return Ok(response);
        }

        return BadRequest("errors during refreshing the token");
    }

    [HttpPost("RegisterCliente")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCliente([FromBody] RegisterUserRequest request)
    {
        var response = await authenticationService.RegisterClienteAsync(request);
        if (response.Succeeded)
        {
            return Ok("Utente registrato con successo");
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPost("RegisterStaff")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterStaff([FromBody] RegisterUserRequest request)
    {
        var response = await authenticationService.RegisterStaffAsync(request);
        if (response.Succeeded)
        {
            return Ok("Utente registrato con successo");
        }
        else
        {
            return BadRequest(response);
        }
    }

    [HttpPut("UpdatePassword")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
    {
        var response = await authenticationService.UpdatePasswordAsync(request);
        if (response.Succeeded)
        {
            return Ok("password cambiata con successo");
        }

        return BadRequest(response);
    }
}