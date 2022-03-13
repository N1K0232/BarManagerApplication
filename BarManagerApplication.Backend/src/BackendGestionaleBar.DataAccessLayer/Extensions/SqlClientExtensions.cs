using BackendGestionaleBar.DataAccessLayer.Clients;
using BackendGestionaleBar.DataAccessLayer.Internal;
using BackendGestionaleBar.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Extensions
{
    public static class SqlClientExtensions
    {
        public static Task<int> FillAsync(this SqlDataAdapter adapter, DataTable dataTable)
        {
            int result;

            try
            {
                result = adapter.Fill(dataTable);
            }
            catch (InvalidOperationException)
            {
                result = -1;
            }

            return Task.FromResult(result);
        }

        public static IServiceCollection AddSqlServer(this IServiceCollection services, string connectionStringHash)
        {
            services.AddScoped<IDatabase, Database>(_ =>
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
            services.AddScoped<IDatabase, Database>(_ =>
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