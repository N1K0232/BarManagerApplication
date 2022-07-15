using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authorization;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    [HttpDelete("Delete")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await orderService.DeleteAsync(id);
        return Ok("order deleted successfully");
    }

    [HttpGet("GetOrders")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await orderService.GetAsync();
        return orders != null ? Ok(orders) : NotFound("no order found");
    }

    [HttpGet("YourOrder")]
    [RoleAuthorize(RoleNames.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> YourOrder()
    {
        var order = await orderService.GetYourOrderAsync();
        return order != null ? Ok(order) : NotFound("You didn't register any order");
    }

    [HttpGet("GetTotalPrice")]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTotalPrice()
    {
        var totalPrice = await orderService.GetTotalPriceAsync();
        return Ok($"you have to pay {totalPrice}€");
    }

    [HttpPost("Save")]
    [RoleAuthorize(RoleNames.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Save([FromBody] SaveOrderRequest request)
    {
        var savedOrder = await orderService.SaveAsync(request);
        return Ok(savedOrder);
    }
}