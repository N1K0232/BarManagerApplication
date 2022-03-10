using BarApplication.APIClient.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Forms;

namespace BarManager.Core.APIClient.Test
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices)
                .Build();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = host.Services.GetService<MainForm>();
            Application.Run(mainForm);
        }

        private static void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
        {
            services.AddIdentityClient();
            services.AddSingleton<MainForm>();
        }
    }
}