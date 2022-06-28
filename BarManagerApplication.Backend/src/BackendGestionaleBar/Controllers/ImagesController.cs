using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authorization;
using BackendGestionaleBar.BusinessLayer.Services.Common;
using BackendGestionaleBar.Extensions;
using BackendGestionaleBar.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ImagesController : ControllerBase
{
	private readonly IImageService imageService;

	public ImagesController(IImageService imageService)
	{
		this.imageService = imageService;
	}

	[HttpDelete("Delete")]
	[RoleAuthorize(RoleNames.Administrator)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await imageService.DeleteAsync(id);
		return NoContent();
	}

	[HttpGet("GetImages")]
	[RoleAuthorize(RoleNames.Administrator)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(string path = null)
	{
		var images = await imageService.GetAsync(path);
		return images != null ? Ok(images) : NotFound("no image found");
	}

	[HttpGet("GetImage/{id:guid}")]
	[RoleAuthorize(RoleNames.Administrator)]
	public async Task<IActionResult> Get(Guid id)
	{
		var stream = await imageService.GetAsync(id);
		return stream != null ? File(stream.Stream, stream.ContentType) : NotFound("no image found");
	}

	[HttpPost("Upload")]
	[RoleAuthorize(RoleNames.Administrator)]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Upload([FromForm] UploadImageRequest request)
	{
		await imageService.UploadAsync(request.ToStreamFileContent());
		return NoContent();
	}
}