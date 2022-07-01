using BackendGestionaleBar.Shared.Responses;

namespace BackendGestionaleBar.WeatherClient;
public interface IWeatherForecastService
{
    Task<WeatherForecastResponse> GetAsync(string city);
}