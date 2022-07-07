using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authorization;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
	private readonly ICategoryService categoryService;

	public CategoriesController(ICategoryService categoryService)
	{
		this.categoryService = categoryService;
	}

	[HttpDelete("Delete")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await categoryService.DeleteAsync(id);
		return Ok("categories successfully deleted");
	}

	[HttpGet("Get")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(string name = null)
	{
		var category = await categoryService.GetAsync(name);
		return category != null ? Ok(category) : NotFound("no category found");
	}

	[HttpPost("Save")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Save([FromBody] SaveCategoryRequest request)
	{
		var savedCategory = await categoryService.SaveAsync(request);
		return Ok(savedCategory);
	}
}