using MeteoClient.Core;
using Microsoft.Extensions.DependencyInjection;

namespace MeteoClient.DependencyInjection
{
    public static class WeatherClientExtensions
    {
        public static IHttpClientBuilder AddWeatherClient(this IServiceCollection services)
        {
            var httpClientBuilder = services.AddHttpClient<IWeatherClient, WeatherClient>(httpClient =>
            {
                return new WeatherClient(httpClient);
            });

            return httpClientBuilder;
        }
    }
}