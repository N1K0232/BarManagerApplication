using BackendGestionaleBar.DataAccessLayer;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDatabase, Database>(_ =>
        {
            Database database = new(connectionString);
            database.InstanceConnection();
            return database;
        });

        return services;
    }
}