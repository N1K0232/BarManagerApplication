using BackendGestionaleBar.Abstractions;
using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

public class ImagesController : ApiController
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
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await imageService.DeleteAsync(id);
		return NoContent();
	}

	[HttpGet("GetImages")]
	[RoleAuthorize(RoleNames.Administrator)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
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
	[Consumes("multipart/form-data")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Upload([FromForm] UploadImageRequest request)
	{
		var savedImage = await imageService.UploadAsync(request.ToStreamFileContent());
		return Ok(savedImage);
	}
}