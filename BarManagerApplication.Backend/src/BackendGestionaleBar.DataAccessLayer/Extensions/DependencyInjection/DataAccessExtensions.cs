using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;

namespace BackendGestionaleBar.DataAccessLayer.Extensions.DependencyInjection
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, Action<DataContextOptions> action)
        {
            var settings = new DataContextOptions();
            action.Invoke(settings);
            var connection = new SqlConnection(settings.ConnectionString);

            services.AddScoped<IDataContext>(_ =>
            {
                var dataContext = new DataContext
                {
                    Connection = connection
                };
                return dataContext;
            });

            return services;
        }
    }
}