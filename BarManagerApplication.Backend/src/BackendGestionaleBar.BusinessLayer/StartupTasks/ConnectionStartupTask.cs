using BackendGestionaleBar.DataAccessLayer.Clients;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            using var database = scope.ServiceProvider.GetRequiredService<IDatabase>();
            using var connection = database.Connection;
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ConnectionStartupTask>>();

            try
            {
                await connection.OpenAsync(cancellationToken);
                await connection.CloseAsync();
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred while connecting to the database");
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "An error occurred while connecting to the database");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}