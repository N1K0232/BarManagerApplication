using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authorization;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

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

    [HttpDelete("Delete")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await productService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("GetMenu")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMenu()
    {
        var menu = await productService.GetMenuAsync();
        return Ok(menu);
    }

    [HttpGet("Get")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string name = null)
    {
        var products = await productService.GetAsync(name);
        return products != null ? Ok(products) : NotFound("no product found");
    }

    [HttpPost("Save")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Save([FromBody] SaveProductRequest request)
    {
        var savedProduct = await productService.SaveAsync(request);
        return Ok(savedProduct);
    }
}