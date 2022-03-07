using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackendGestionaleBar.BusinessLayer.StartupTasks
{
    public class AuthenticationStartupTask : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public AuthenticationStartupTask(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var roleNames = new string[] { RoleNames.Administrator, RoleNames.Cliente };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var admin = new ApplicationUser
            {
                FirstName = "Nicola",
                LastName = "Silvestri",
                BirthDate = DateTime.Parse("22/10/2002"),
                Email = "ns.nicolasilvestri@gmail.com",
                UserName = "N1K0232"
            };

            await CheckCreateUserAsync(admin, "NicoSilve22!", RoleNames.Administrator, RoleNames.Cliente);

            async Task CheckCreateUserAsync(ApplicationUser user, string password, params string[] roles)
            {
                var dbUser = await userManager.FindByNameAsync(user.UserName);
                if (dbUser == null)
                {
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRolesAsync(user, roles);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}