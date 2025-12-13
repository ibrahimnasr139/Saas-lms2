using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Health;
using Infrastructure.Repositories;
using Infrastructure.Seeders;
using Infrastructure.Services;
using Infrastructure.Services.AuthServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class InfrastructureService
    {
        public static void AddInfrastructureServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
            });
            builder.Services.AddHybridCache();
            builder.Services.AddOptions<JwtOptions>()
                .BindConfiguration(nameof(JwtOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            builder.Services.AddOptions<MailOptions>()
                .BindConfiguration(nameof(MailOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UsePostgreSqlStorage(action => action.UseNpgsqlConnection(configuration.GetConnectionString("HangfireConnection")))
            );

            builder.Services.AddHangfireServer();

            builder.Services.AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!)
                .AddHangfire(options => options.MinimumAvailableServers = 1)
                .AddRedis(configuration.GetConnectionString("RedisConnection")!)
                .AddCheck<MailHealthCheck>("mail service");
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddScoped<IRefreshRepository, RefreshRepository>();

            builder.Services.AddScoped<ISeeder, Seeder>();
            builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<ITenantRepository, TenantRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}