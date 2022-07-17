using BackendGestionaleBar.Abstractions.Controllers;
using BackendGestionaleBar.Abstractions.Filters;
using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.WeatherClient;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

public class WeatherForecastController : ApiController
{
	private readonly IWeatherForecastService weatherForecastService;

	public WeatherForecastController(IWeatherForecastService weatherForecastService)
	{
		this.weatherForecastService = weatherForecastService;
	}

	[HttpGet("GetWeather")]
	[RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetWeather(string city)
	{
		var response = await weatherForecastService.GetAsync(city);
		return response != null ? Ok(response) : NotFound("not valid city");
	}
}