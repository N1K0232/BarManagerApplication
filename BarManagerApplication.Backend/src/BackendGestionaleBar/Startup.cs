using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authorization.Handlers;
using BackendGestionaleBar.Authorization.Requirements;
using BackendGestionaleBar.BusinessLayer.Extensions;
using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.BusinessLayer.StartupTasks;
using BackendGestionaleBar.Contracts;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Services;
using Hellang.Middleware.ProblemDetails;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

namespace BackendGestionaleBar;

public class Startup
{
    private readonly IServiceCollection services;
    private readonly IConfiguration configuration;
    private readonly IHostBuilder host;

    public Startup(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        this.services = services;
        this.configuration = configuration;
        this.host = host;
    }

    public void ConfigureServices()
    {
        host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
        });

        var jwtSettings = Configure<JwtSettings>(nameof(JwtSettings));

        services.AddProblemDetails();
        services.AddMapperProfiles();
        services.AddValidators();
        services.AddControllers()
            .AddJsonOptions(options =>
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
        })
        .AddFluentValidationRulesToSwagger(options =>
        {
            options.SetNotNullableIfMinLengthGreaterThenZero = true;
        });

        string hash = configuration.GetConnectionString("SqlConnection");
        byte[] bytes = Convert.FromBase64String(hash);
        string connectionString = Encoding.UTF8.GetString(bytes);
        services.AddSqlServer<AuthenticationDataContext>(connectionString);
        services.AddSqlServer<DataContext>(connectionString);
        services.AddScoped<IDataContext>(services => services.GetRequiredService<DataContext>());

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
        })
        .AddEntityFrameworkStores<AuthenticationDataContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
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

        services.AddScoped<IAuthorizationHandler, UserActiveHandler>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, HttpUserService>();

        services.AddHostedService<AuthenticationStartupTask>();
    }

    private T Configure<T>(string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        var settings = section.Get<T>();
        services.Configure<T>(section);
        return settings;
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseProblemDetails();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendGestionaleBar v1"));
        app.UseSerilogRequestLogging(options =>
        {
            options.IncludeQueryInRequestPath = true;
        });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}