using BackendGestionaleBar.Authentication;
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
            using var scope = serviceProvider.CreateScope();
            using var authenticationDataContext = scope.ServiceProvider.GetRequiredService<AuthenticationDataContext>();
            bool canConnect = await authenticationDataContext.Database.CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                throw new InvalidOperationException("Can't connect to the database");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}