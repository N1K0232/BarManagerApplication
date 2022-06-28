using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BackendGestionaleBar.BusinessLayer.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidation(options =>
        {
            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        });
        return services;
    }
}