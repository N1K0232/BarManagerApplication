using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackendGestionaleBar.Authentication.StartupTasks;

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
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AuthenticationRole>>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AuthenticationUser>>();

        string[] roleNames = new string[] { RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer };

        foreach (string roleName in roleNames)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new AuthenticationRole(roleName));
            }
        }

        var nicoAdminUser = new AuthenticationUser
        {
            Name = "Nicola Silvestri",
            DateOfBirth = DateTime.Parse("22/10/2002"),
            Email = "ns.nicolasilvestri@gmail.com",
            UserName = "N1K0232",
            PhoneNumber = "3319907702"
        };

        var mamtaAdminUser = new AuthenticationUser
        {
            Name = "Mamta",
            DateOfBirth = DateTime.Parse("03/03/2006"),
            Email = "ns.nicolasilvestri@libero.it",
            UserName = "R1K023",
            PhoneNumber = "3319907703"
        };

        await CheckCreateUserAsync(nicoAdminUser, "Tmljb0xvdmVzTWFtdGExOTE2IQ==", RoleNames.Administrator, RoleNames.Staff);
        await CheckCreateUserAsync(mamtaAdminUser, "TWFtdGFMb3Zlc05pY28xNjE5IQ==", RoleNames.Administrator, RoleNames.Staff);

        async Task CheckCreateUserAsync(AuthenticationUser user, string password, params string[] roles)
        {
            var dbUser = await userManager.FindByNameAsync(user.UserName);
            if (dbUser == null)
            {
                var result = await userManager.CreateAsync(user, StringConverter.GetString(password));
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}