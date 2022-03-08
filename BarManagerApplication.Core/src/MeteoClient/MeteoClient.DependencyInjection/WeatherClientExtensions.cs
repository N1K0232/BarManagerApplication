using MeteoClient.Core;
using Microsoft.Extensions.DependencyInjection;

namespace MeteoClient.DependencyInjection
{
    public static class WeatherClientExtensions
    {
        public static IServiceCollection AddWeatherClient(this IServiceCollection services)
        {
            services.AddScoped<IWeatherClient, WeatherClient>();
            return services;
        }
    }
}