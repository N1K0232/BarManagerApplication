using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.DataAccessLayer.Clients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.StartupTasks
{
    public class ConnectionStartupTask : IHostedService, IDisposable
    {
        private readonly IServiceScope scope;

        private AuthenticationDbContext authenticationDbContext;
        private ApplicationDbContext applicationDbContext;

        public ConnectionStartupTask(IServiceProvider serviceProvider)
        {
            scope = serviceProvider.CreateScope();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            authenticationDbContext = scope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
                if (applicationDbContext != null)
                {
                    applicationDbContext.Dispose();
                }
                if (authenticationDbContext != null)
                {
                    authenticationDbContext.Dispose();
                }
            }
        }
    }
}