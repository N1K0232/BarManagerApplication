﻿using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService identityService;

    public AuthenticationController(IAuthenticationService identityService)
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

    [HttpPost("RegisterCliente")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCliente([FromBody] RegisterUserRequest request)
    {
        var response = await identityService.RegisterClienteAsync(request);
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
        var response = await identityService.RegisterStaffAsync(request);
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
        var response = await identityService.UpdatePasswordAsync(request);
        if (response.Succeeded)
        {
            return Ok("password cambiata con successo");
        }

        return BadRequest(response);
    }
}