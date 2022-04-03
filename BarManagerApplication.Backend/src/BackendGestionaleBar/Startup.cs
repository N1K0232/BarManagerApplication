using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.Requirements;
using BackendGestionaleBar.BusinessLayer.MapperConfigurations;
using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.BusinessLayer.StartupTasks;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.DataAccessLayer.Extensions.DependencyInjection;
using BackendGestionaleBar.Helpers;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;

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
            var jwtSettings = Configure<JwtSettings>(nameof(JwtSettings));

            services.AddProblemDetails();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BackendGestionaleBar", Version = "v1" });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert the bearer token",
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            });

            services.AddDbContext<AuthenticationDataContext>(options =>
            {
                string hash = Configuration.GetConnectionString("SqlConnection");
                string connectionString = StringConverter.GetString(hash);
                options.UseSqlServer(connectionString, dbOptions =>
                {
                    dbOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), null);
                });
            });
            services.AddDbContext<DataContext>(options =>
            {
                string hash = Configuration.GetConnectionString("SqlConnection");
                string connectionString = StringConverter.GetString(hash);
                options.UseSqlServer(connectionString, dbOptions =>
                {
                    dbOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), null);
                });
            });
            services.AddScoped<IReadOnlyDataContext>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<DataContext>();
            });
            services.AddScoped<IDataContext>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<DataContext>();
            });
            services.AddDatabase(options =>
            {
                string hash = Configuration.GetConnectionString("SqlConnection");
                string connectionString = StringConverter.GetString(hash);
                options.ConnectionString = connectionString;
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
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

            services.AddScoped<IAuthorizationHandler, UserActiveHandler>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddHostedService<ConnectionStartupTask>();
            services.AddHostedService<AuthenticationStartupTask>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddAuthorization(options =>
            {
                var policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
                policyBuilder.Requirements.Add(new UserActiveRequirement());
                options.FallbackPolicy = options.DefaultPolicy = policyBuilder.Build();
            });

            services.AddAutoMapper(typeof(ProductMapperProfile).Assembly);
            services.AddFluentValidation();

            T Configure<T>(string sectionName) where T : class
            {
                var section = Configuration.GetSection(sectionName);
                var settings = section.Get<T>();
                services.Configure<T>(section);
                return settings;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendGestionaleBar v1"));
            }

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
}