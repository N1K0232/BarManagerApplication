using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            using var connection = serviceProvider.GetRequiredService<SqlConnection>();

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
        {
            throw new NotImplementedException();
        }
    }
}