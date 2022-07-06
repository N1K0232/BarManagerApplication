using BackendGestionaleBar.BusinessLayer.Extensions;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.Extensions;
using BackendGestionaleBar.Security;
using BackendGestionaleBar.StorageProviders.Extensions;
using BackendGestionaleBar.WeatherClient.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = Configure<JwtSettings>(nameof(JwtSettings));
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddProblemDetails();
builder.Services.AddMapperProfiles();
builder.Services.AddValidators();
builder.Services.AddSwaggerSettings();

string connectionString = StringConverter.GetString(builder.Configuration.GetConnectionString("SqlConnection"));
builder.Services.AddDataContext(connectionString);

builder.Services.AddIdentitySettings(jwtSettings);

builder.Services.AddServices();
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
app.UseSwaggerSettings();
app.UseSerilogRequestLogging(options =>
{
    options.IncludeQueryInRequestPath = true;
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseIdentitySettings();
app.MapControllers();
await app.RunAsync();