using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackendGestionaleBar.BusinessLayer.StartupTasks;

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
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = new string[] { RoleNames.Administrator, RoleNames.Staff, RoleNames.Cliente };

        foreach (string roleName in roleNames)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName));
            }
        }

        var admin = new ApplicationUser
        {
            FirstName = "Nicola",
            LastName = "Silvestri",
            DateOfBirth = DateTime.Parse("22/10/2002"),
            Email = "ns.nicolasilvestri@gmail.com",
            UserName = "N1K0232",
            PhoneNumber = "3319907702"
        };

        await CheckCreateUserAsync(admin, "NicoSilve22!", RoleNames.Administrator, RoleNames.Staff);

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

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}