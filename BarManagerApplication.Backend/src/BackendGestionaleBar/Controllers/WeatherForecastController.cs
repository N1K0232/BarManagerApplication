using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authorization;
using BackendGestionaleBar.WeatherClient;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WeatherForecastController : ControllerBase
{
	private readonly IWeatherForecastService weatherForecastService;

	public WeatherForecastController(IWeatherForecastService weatherForecastService)
	{
		this.weatherForecastService = weatherForecastService;
	}

	[HttpGet("GetWeather")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Cliente)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetWeather(string city)
	{
		var response = await weatherForecastService.GetAsync(city);
		return response != null ? Ok(response) : NotFound("not valid city");
	}
}