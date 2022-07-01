using BackendGestionaleBar.WeatherClient.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace BackendGestionaleBar.WeatherClient.DependencyInjection;

public static class WeatherForecastExtensions
{
    public static IHttpClientBuilder AddWeatherService(this IServiceCollection services, Action<WeatherClientSettings> configuration)
    {
        var settings = new WeatherClientSettings();
        configuration.Invoke(settings);

        var httpClientBuilder = services.AddHttpClient<IWeatherForecastService, WeatherForecastService>(httpClient =>
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return new WeatherForecastService(settings, httpClient);
        });

        return httpClientBuilder;
    }
}