using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Filters;
using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("GetMenu")]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Cliente)]
        [ProducesResponseType(200, Type = typeof(List<Menu>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMenu()
        {
            List<Menu> menu = await productService.GetMenuAsync();
            if (menu != null && menu.Count > 0)
            {
                return Ok(menu);
            }

            return NotFound("No product found");
        }

        [HttpGet("GetProduct")]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Cliente)]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProduct(Guid idProduct)
        {
            var product = await productService.GetProductAsync(idProduct);
            if (product == null)
            {
                return NotFound("No product found");
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpPost("RegisterProduct")]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> RegisterProduct([FromBody] SaveProductRequest request)
        {
            var savedProduct = await productService.SaveProductAsync(request);
            if (savedProduct != null)
            {
                return Ok(savedProduct);
            }

            return BadRequest("errors during registration");
        }
    }
}