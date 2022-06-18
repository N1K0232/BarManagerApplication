namespace BackendGestionaleBar;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Startup startup = new(builder.Services, builder.Configuration, builder.Host);
        startup.ConfigureServices();
        WebApplication application = builder.Build();
        startup.Configure(application);
        await application.RunAsync();
    }
}