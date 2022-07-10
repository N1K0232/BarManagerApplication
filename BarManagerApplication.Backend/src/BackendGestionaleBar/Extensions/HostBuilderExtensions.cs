using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.StartupTasks;
using BackendGestionaleBar.Authorization.Handlers;
using BackendGestionaleBar.Authorization.Requirements;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.DataAccessLayer;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace BackendGestionaleBar.Extensions;

public static class HostBuilderExtensions
{
    public static IServiceCollection AddSwaggerSettings(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendGestionaleBar", Version = "v1" });
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insert the bearer token",
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.ApiKey
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }).AddFluentValidationRulesToSwagger(options =>
        {
            options.SetNotNullableIfMinLengthGreaterThenZero = true;
        });

        return services;
    }

    public static IServiceCollection AddDataContext(this IServiceCollection services, string connectionString)
    {
        services.AddSqlServer<AuthenticationDataContext>(connectionString);
        services.AddDbContext<IDataContext, DataContext>(options =>
        {
            options.UseSqlServer(connectionString, dbOptions =>
            {
                dbOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(2), null);
            });
        });

        return services;
    }

    public static IServiceCollection AddIdentitySettings(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
        }).AddEntityFrameworkStores<AuthenticationDataContext>().AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            var policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
            policyBuilder.Requirements.Add(new UserActiveRequirement());
            options.FallbackPolicy = options.DefaultPolicy = policyBuilder.Build();
        });

        services.AddHostedService<AuthenticationStartupTask>();
        services.AddScoped<IAuthorizationHandler, UserActiveHandler>();

        return services;
    }

    public static IApplicationBuilder UseSwaggerSettings(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendGestionaleBar v1"));
        return app;
    }

    public static IApplicationBuilder UseIdentitySettings(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}