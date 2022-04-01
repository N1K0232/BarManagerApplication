using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;

namespace BackendGestionaleBar.DataAccessLayer.Extensions.DependencyInjection
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, Action<DataContextBuilder> action)
        {
            var settings = new DataContextBuilder();
            action.Invoke(settings);

            services.AddScoped<IDataContext>(_ =>
            {
                var dataContext = new DataContext
                {
                    Connection = new SqlConnection(settings.ConnectionString)
                };
                return dataContext;
            });

            return services;
        }
    }
}