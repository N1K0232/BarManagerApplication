using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.StartupTasks
{
    public sealed class ConnectionStartupTask : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public ConnectionStartupTask(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            using var connection = scope.ServiceProvider.GetRequiredService<SqlConnection>();

            try
            {
                await connection.OpenAsync(cancellationToken);
                await connection.CloseAsync();
            }
            catch (SqlException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}