using BackendGestionaleBar.Security;
using BackendGestionaleBar.Shared.Responses;
using BackendGestionaleBar.WeatherClient.Models;
using BackendGestionaleBar.WeatherClient.Settings;
using Newtonsoft.Json.Linq;

namespace BackendGestionaleBar.WeatherClient;

internal class WeatherForecastService : IWeatherForecastService, IDisposable
{
	private readonly WeatherClientSettings weatherClientSettings;
	private readonly HttpClient httpClient;

	private string url = "";
	private string result = null;

	private string text = "";
	private string icon = "";
	private double? temperature = null;

	public WeatherForecastService(WeatherClientSettings weatherClientSettings, HttpClient httpClient)
	{
		this.weatherClientSettings = weatherClientSettings;
		this.httpClient = httpClient;
		CreateUrl();
	}

	private void CreateUrl()
	{
		string baseUrl = StringConverter.GetString(weatherClientSettings.BaseUrl);
		string apiKey = StringConverter.GetString(weatherClientSettings.ApiKey);
		url = $"{baseUrl}{apiKey}";
	}

	public async Task<WeatherForecastResponse> GetAsync(string city)
	{
		Location location = null;

		url += $"&q={city}&aqi=no";

		try
		{
			result = await httpClient.GetStringAsync(url);
			text = "";
		}
		catch (HttpRequestException)
		{
			text = "Inserisci una città valida";
		}
		catch (InvalidOperationException)
		{
			text = "Error";
		}

		if (!string.IsNullOrEmpty(text))
		{
			return null;
		}

		Exception e = null;
		JObject deserialized = JObject.Parse(result);

		try
		{
			temperature = deserialized["current"]?["temp_c"]?.ToObject<double?>();
			text = deserialized["current"]?["condition"]?["text"]?.ToString();
			icon = deserialized["current"]?["condition"]?["icon"]?.ToString();
			location = deserialized["location"]?.ToObject<Location>();
		}
		catch (NullReferenceException ex)
		{
			e = ex;
		}
		catch (InvalidOperationException ex)
		{
			e = ex;
		}

		if (e != null || location == null)
		{
			return null;
		}

		var response = new WeatherForecastResponse
		{
			Temperature = temperature,
			Text = text,
			Icon = icon,
			Location = location.Name,
			Region = location.Region,
			Country = location.Country
		};

		return response;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	private void Dispose(bool disposing)
	{
		if (disposing && httpClient != null)
		{
			httpClient.Dispose();
		}
	}
}