using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace BackendGestionaleBar.DataAccessLayer.Extensions.DependencyInjection
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, Action<DatabaseOptionsBuilder> configuration)
        {
            services.AddSqlConnection(configuration);
            services.AddScoped<IDatabase, Database>(serviceProvider =>
            {
                var connection = serviceProvider.GetRequiredService<SqlConnection>();
                var database = new Database();
                database.Connection = connection;
                return database;
            });
            return services;
        }

        private static IServiceCollection AddSqlConnection(this IServiceCollection services, Action<DatabaseOptionsBuilder> configuration)
        {
            var builder = new DatabaseOptionsBuilder();
            configuration.Invoke(builder);

            services.AddScoped<SqlConnection>(_ =>
            {
                return new SqlConnection(builder.ConnectionString);
            });

            return services;
        }
    }
}