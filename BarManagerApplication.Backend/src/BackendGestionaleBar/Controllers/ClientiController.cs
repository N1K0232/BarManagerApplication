using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BackendGestionaleBar.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClientiController : ControllerBase
    {
        private readonly IClienteService clienteService;

        public ClientiController(IClienteService clienteService)
        {
            this.clienteService = clienteService;
        }

        [HttpGet("GetCliente")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCliente(Guid idCliente)
        {
            if (idCliente == Guid.Empty)
            {
                return BadRequest("Invalid id");
            }

            var cliente = await clienteService.GetClienteAsync(idCliente);
            if (cliente == null)
            {
                return NotFound("il cliente non esiste");
            }
            else
            {
                return Ok(cliente);
            }
        }

        [HttpPost("RegisterCliente")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RegisterCliente([FromBody] RegisterClienteRequest request)
        {
            var response = await clienteService.RegisterClienteAsync(request);
            if (response.Succeeded)
            {
                return Ok("cliente registrato con successo");
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}