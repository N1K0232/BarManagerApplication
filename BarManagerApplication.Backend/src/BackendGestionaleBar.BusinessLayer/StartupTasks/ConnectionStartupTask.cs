using BackendGestionaleBar.Authentication;
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
            using var dataContext = scope.ServiceProvider.GetService<AuthenticationDataContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ConnectionStartupTask>>();

            var canConnect = await dataContext.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                logger.LogError("problem occurred while connecting database");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}