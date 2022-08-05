using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackendGestionaleBar.Authentication.StartupTasks;

public sealed class AuthenticationStartupTask : IHostedService
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

        await CheckCreateUserAsync(nicoAdminUser, "Tmljb0xvdmVzTWFtdGExOTE2IQ==");
        await CheckCreateUserAsync(mamtaAdminUser, "TWFtdGFMb3Zlc05pY28xNjE5IQ==");

        async Task CheckCreateUserAsync(AuthenticationUser user, string password)
        {
            await userManager.RegisterAsync(user, StringConverter.GetString(password), RoleNames.Administrator, RoleNames.Staff);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}