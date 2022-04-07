using BackendGestionaleBar.BusinessLayer.MapperConfigurations;
using BackendGestionaleBar.BusinessLayer.Services;
using BackendGestionaleBar.BusinessLayer.Settings;
using BackendGestionaleBar.BusinessLayer.StartupTasks;
using BackendGestionaleBar.BusinessLayer.Validators;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.DataAccessLayer.Entities;
using BackendGestionaleBar.DataAccessLayer.Extensions.DependencyInjection;
using BackendGestionaleBar.DataAccessLayer.Requirements;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
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

namespace BackendGestionaleBar
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = Configure<JwtSettings>(nameof(JwtSettings));

            services.AddProblemDetails();
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

            services.AddDbContext<AuthenticationDataContext>(options =>
            {
                options.UseSqlServer(GetConnectionString(), dbOptions =>
                {
                    dbOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), null);
                });
            });
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(GetConnectionString(), dbOptions =>
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
                options.ConnectionString = GetConnectionString();
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
            services.AddScoped<IOrderService, OrderService>();

            services.AddAuthorization(options =>
            {
                var policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
                policyBuilder.Requirements.Add(new UserActiveRequirement());
                options.FallbackPolicy = options.DefaultPolicy = policyBuilder.Build();
            });

            services.AddAutoMapper(typeof(ProductMapperProfile).Assembly);
            services.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<SaveOrderValidator>();
            });

            T Configure<T>(string sectionName) where T : class
            {
                var section = Configuration.GetSection(sectionName);
                var settings = section.Get<T>();
                services.Configure<T>(section);
                return settings;
            }
        }

        private string GetConnectionString()
        {
            string hash;

            if (Environment.IsDevelopment())
            {
                hash = Configuration.GetConnectionString("SqlConnection");
            }
            else
            {
                hash = Configuration.GetConnectionString("AzureConnection");
            }

            var bytes = Convert.FromBase64String(hash);
            return Encoding.UTF8.GetString(bytes);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendGestionaleBar v1"));
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