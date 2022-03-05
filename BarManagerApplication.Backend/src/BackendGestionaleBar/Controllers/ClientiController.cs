using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("RegisterCliente")]
        [ProducesResponseType(204)]
        //[ProducesResponseType(200)]
        public async Task<IActionResult> RegisterCliente([FromBody] RegisterClienteRequest request)
        {
            var response = await clienteService.RegisterClienteAsync(request);
            return NoContent();
        }
    }
}