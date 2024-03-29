﻿using BackendGestionaleBar.Abstractions.Controllers;
using BackendGestionaleBar.Abstractions.Filters;
using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

public class UmbrellasController : ApiController
{
	private readonly IUmbrellaService umbrellaService;

	public UmbrellasController(IUmbrellaService umbrellaService)
	{
		this.umbrellaService = umbrellaService;
	}

	[HttpDelete("Delete")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await umbrellaService.DeleteAsync(id);
		return Ok("Successfully deleted");
	}

	[HttpGet("Get")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(string coordinates = null)
	{
		var umbrellas = await umbrellaService.GetAsync(coordinates);
		return umbrellas != null ? Ok(umbrellas) : NotFound("No umbrella found");
	}

	[HttpPost("Save")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> Save([FromBody] SaveUmbrellaRequest request)
	{
		var savedUmbrella = await umbrellaService.SaveAsync(request);
		return Ok(savedUmbrella);
	}
}