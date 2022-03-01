using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.StartupTasks
{
    public sealed class ConnectionStartupTasks : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public ConnectionStartupTasks(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            SqlConnection connection;
            Exception e = null;

            try
            {
                connection = scope.ServiceProvider.GetRequiredService<SqlConnection>();
            }
            catch (InvalidOperationException ex)
            {
                e = ex;
                connection = null;
            }

            if (e != null)
            {
                await StopAsync(cancellationToken);
                return;
            }

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
                await StopAsync(cancellationToken);
                return;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}