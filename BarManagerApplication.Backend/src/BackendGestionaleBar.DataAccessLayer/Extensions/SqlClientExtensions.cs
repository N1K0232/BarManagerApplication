using BackendGestionaleBar.DataAccessLayer.Clients;
using BackendGestionaleBar.Helpers;
using Microsoft.Data.SqlClient;
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
    }
}