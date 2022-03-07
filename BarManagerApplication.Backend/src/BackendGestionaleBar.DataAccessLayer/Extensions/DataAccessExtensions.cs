using BackendGestionaleBar.DataAccessLayer.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionStringHash)
        {
            services.AddScoped<IDatabase>(_ =>
            {
                return new Database(connectionStringHash);
            });

            return services;
        }
    }
}