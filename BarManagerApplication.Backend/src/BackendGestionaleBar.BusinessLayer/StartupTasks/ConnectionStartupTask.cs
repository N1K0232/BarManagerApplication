using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.DataAccessLayer.Clients;
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
            using var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            using var authenticationDbContext = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

            bool canConnect = await applicationDbContext.Database.CanConnectAsync(cancellationToken);
            if (canConnect)
            {
                canConnect = await authenticationDbContext.Database.CanConnectAsync(cancellationToken);
            }

            if (!canConnect)
            {
                throw new InvalidOperationException("can't connect to database");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}