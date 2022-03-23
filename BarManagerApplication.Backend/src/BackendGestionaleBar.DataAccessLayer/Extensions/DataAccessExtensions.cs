using BackendGestionaleBar.DataAccessLayer.Clients;
using BackendGestionaleBar.DataAccessLayer.Internal;
using BackendGestionaleBar.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, string connectionStringHash)
        {
            services.AddDbContext<IDataContext, DataContext>(options =>
            {
                string connectionString = StringConverter.GetString(connectionStringHash);
                options.UseSqlServer(connectionString, dbOptions =>
                {
                    dbOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), null);
                });
            });

            return services;
        }

        public static IServiceCollection AddSqlServer(this IServiceCollection services, string connectionStringHash)
        {
            services.AddScoped<IDatabase>(_ =>
            {
                string connectionString = StringConverter.GetString(connectionStringHash);
                Database database = new();
                SqlConnection connection = new(connectionString);
                database.Connection = connection;
                return database;
            });

            return services;
        }

        public static IServiceCollection AddAzureSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDatabase>(_ =>
            {
                Database database = new();
                SqlConnection connection = ConnectionHelper.CreateAzureConnection(configuration);
                database.Connection = connection;
                return database;
            });
            return services;
        }
    }
}