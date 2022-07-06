using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.StartupTasks;
using BackendGestionaleBar.Authorization.Handlers;
using BackendGestionaleBar.Authorization.Requirements;
using BackendGestionaleBar.BusinessLayer.Extensions;
using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.BusinessLayer.Services.Common;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.Contracts;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Security;
using BackendGestionaleBar.Services;
using BackendGestionaleBar.StorageProviders.Extensions;
using BackendGestionaleBar.WeatherClient.DependencyInjection;
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

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

var jwtSettings = Configure<JwtSettings>(nameof(JwtSettings));

builder.Services.AddProblemDetails();
builder.Services.AddMapperProfiles();
builder.Services.AddValidators();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(options =>
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

string connectionString = StringConverter.GetString(builder.Configuration.GetConnectionString("SqlConnection"));
builder.Services.AddSqlServer<AuthenticationDataContext>(connectionString);
builder.Services.AddDbContext<IBarManagerDataContext, BarManagerDataContext>(options =>
{
    options.UseSqlServer(connectionString, dbOptions =>
    {
        dbOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(2), null);
    });
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
}).AddEntityFrameworkStores<AuthenticationDataContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
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
builder.Services.AddAuthorization(options =>
{
    var policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
    policyBuilder.Requirements.Add(new UserActiveRequirement());
    options.FallbackPolicy = options.DefaultPolicy = policyBuilder.Build();
});

builder.Services.AddHostedService<AuthenticationStartupTask>();

builder.Services.AddScoped<IAuthorizationHandler, UserActiveHandler>();
builder.Services.AddScoped<IUserService, HttpUserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddWeatherService(options =>
{
    options.BaseUrl = builder.Configuration.GetValue<string>("WeatherClientSettings:BaseUrl");
    options.ApiKey = builder.Configuration.GetValue<string>("WeatherClientSettings:ApiKey");
});
builder.Services.AddFileSystemStorageProvider(options =>
{
    options.StorageFolder = builder.Configuration.GetValue<string>("AppSettings:StorageFolder");
});

T Configure<T>(string sectionName) where T : class
{
    var section = builder.Configuration.GetSection(sectionName);
    var settings = section.Get<T>();
    builder.Services.Configure<T>(section);
    return settings;
}

var app = builder.Build();
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
app.MapControllers();
await app.RunAsync();