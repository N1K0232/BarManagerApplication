using Microsoft.Extensions.DependencyInjection;

namespace BackendGestionaleBar.BusinessLayer.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            return services;
        }
    }
}