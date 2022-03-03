using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace BackendGestionaleBar
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            IHostBuilder builder = await CreateHostBuilderAsync(args);
            IHost host = await CreateHostAsync(builder);
            await host.RunAsync();
            host.Dispose();
        }

        private static Task<IHostBuilder> CreateHostBuilderAsync(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return Task.FromResult(builder);
        }

        private static Task<IHost> CreateHostAsync(IHostBuilder builder)
        {
            IHost host = builder.Build();
            return Task.FromResult(host);
        }
    }
}