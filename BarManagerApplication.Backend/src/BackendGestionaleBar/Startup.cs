using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.BusinessLayer.StartupTasks;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;

namespace BackendGestionaleBar
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendGestionaleBar", Version = "v1" });
            });

            services.AddHostedService<ConnectionStartupTask>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IDatabase, Database>();
            services.AddScoped<SqlConnection>(_ =>
            {
                string connectionString = StringConverter.GetString(Configuration.GetConnectionString("SqlConnection"));
                return new SqlConnection(connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendGestionaleBar v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
