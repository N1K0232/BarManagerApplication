using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.Shared.Models.Requests;
using BackendGestionaleBar.Shared.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
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

            return BadRequest();
        }

        [HttpPost("RegisterCliente")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterCliente([FromBody] RegisterRequest request)
        {
            var response = await identityService.RegisterClienteAsync(request);
            if (response.Succeeded)
            {
                return Ok("Utente registrato con successo");
            }
            else
            {
                return BadRequest(response.Errors);
            }
        }

        [HttpPost("RegisterStaff")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterStaff([FromBody] RegisterRequest request)
        {
            var response = await identityService.RegisterStaffAsync(request);
            if (response.Succeeded)
            {
                return Ok("Utente registrato con successo");
            }
            else
            {
                return BadRequest(response.Errors);
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
}