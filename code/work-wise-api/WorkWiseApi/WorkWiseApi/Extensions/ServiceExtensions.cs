using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Entities;
using Contracts;
using Contracts.IServices;
using LoggerService;
using Contracts.IRepository;
using Repository;
using Services;
using AutoMapper;
using ExceptionHandler;

namespace WorkWiseApi.Extensions
{
    /// <summary>
    /// Class <c>Service Extenstions</c> is static consists of service extensions.
    /// Contains static method to configure the services required to run the application.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// ConfigureDBContext method to inject the repository context into IOC with DBContext scope.
        /// </summary>
        /// <paramref name="config">The config paramter used to read attributes in appsettings.json</paramref>
        public static void ConfigureDBContext(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            string connectionString = config["DbConnection:ConnectionString"];
            _ =
                services
                    .AddDbContextPool<RepositoryContext>(o =>
                        o.UseNpgsql(connectionString));
        }

        /// <summary>
        /// This method is used to validate the Authorization token. APIs annotated with Authorize 
        /// must have Authorization header to pass through, otherwise it will return unauthorized.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding
                                        .UTF8
                                        .GetBytes(configuration["Jwt:Key"])),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuerSigningKey = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    o.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        /// <summary>
        /// ConfigureLoggerService to inject logger service inside the .NET Core’s IOC container with 
        /// singleton scope.
        /// </summary>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            _ = services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// ConfigureRepositoryWrapper method to inject the entity repository as scoped instance.
        /// </summary>
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            _ = services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        /// <summary>
        /// ConfigureAutoMapper method to inject the mappings of Entities and DTOs.
        /// </summary>
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _ = services.AddSingleton(mapper);
        }

        /// <summary>
        /// ConfigureServiceWrapper method to inject the services where the business logics are implemented.
        /// </summary>
        public static void ConfigureServiceWrapper(this IServiceCollection services, IConfiguration config)
        {
            _ = services.AddHttpContextAccessor();
            _ = services.AddTransient<UserIdentityService>();
            _ = services.AddScoped<IUserService, UserService>();
            _ = services.AddScoped<IGoalManagementService, GoalManagementService>();
            _ = services.AddScoped<ITaskManagementService, TaskManagementService>();
            _ = services.AddScoped<IDashboardService, DashboardService>();
        }

        /// <summary>
        /// UseHttpStatusCodeExceptionMiddleware method to inject the custom exception middleware.
        /// </summary>
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>();
        }

        /// <summary>
        /// ConfigureDbMigration method to setup auto update of db migrations.
        /// </summary>
        public static void ConfigureDbMigration(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            var logger = services.GetService<ILoggerManager>();
            logger.LogInfo("Db migration configuration");
            try
            {
                DBMigration.UpdateDatabase(services);
                SeedData.Initialize(services);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
            }
        }

        /// <summary>
        /// ConfigureCorsPolicy method to setup cors to allow all origins.
        /// </summary>
        public static void ConfigureCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin() // Allow requests from any origin
                            .AllowAnyMethod() // Allow any HTTP method
                            .AllowAnyHeader() // Allow any HTTP headers
                            .WithMethods("OPTIONS")
                            .WithExposedHeaders("Content-Disposition");

                    });
            });
        }
    }
}
