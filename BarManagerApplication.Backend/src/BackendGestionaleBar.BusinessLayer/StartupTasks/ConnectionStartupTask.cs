using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackendGestionaleBar.BusinessLayer.StartupTasks
{
    public class ConnectionStartupTask : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public ConnectionStartupTask(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            using var connection = scope.ServiceProvider.GetService<SqlConnection>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ConnectionStartupTask>>();

            Exception e = null;
            try
            {
                await connection.OpenAsync(cancellationToken);
                await connection.CloseAsync();
            }
            catch (SqlException ex)
            {
                e = ex;
            }
            catch (InvalidOperationException ex)
            {
                e = ex;
            }

            if (e != null)
            {
                logger.LogError(e, "Can't connect to the database");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}