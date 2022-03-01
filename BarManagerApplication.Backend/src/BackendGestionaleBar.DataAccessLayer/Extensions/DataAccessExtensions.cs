using BackendGestionaleBar.Security;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddSqlConnection(this IServiceCollection services, string connectionStringHash)
        {
            services.AddScoped<SqlConnection>(_ =>
            {
                string connectionString = StringConverter.GetString(connectionStringHash);
                return new SqlConnection(connectionString);
            });

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddScoped<IDatabase, Database>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string[] parameters)
        {
            services.AddScoped<IDatabase>(_ =>
            {
                return new Database(parameters);
            });

            return services;
        }
    }
}