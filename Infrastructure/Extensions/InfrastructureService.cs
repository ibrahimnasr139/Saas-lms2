using Application.Contracts.Files;
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
using Npgsql;

namespace Infrastructure.Extensions
{
    public static class InfrastructureService
    {
        public static void AddInfrastructureServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddDbContextPool<AppDbContext>(options =>
            {
                var connectionString = BuildPostgresConnectionString(configuration);
                options.UseNpgsql(connectionString);
            });
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var redisUrl = Environment.GetEnvironmentVariable("REDIS_URL")
                    ?? configuration.GetConnectionString("RedisConnection");

                if (redisUrl?.StartsWith("redis://") == true)
                {
                    var uri = new Uri(redisUrl);
                    var password = uri.UserInfo.Split(':').LastOrDefault();
                    redisUrl = $"{uri.Host}:{uri.Port},password={password},ssl=false";
                }

                options.Configuration = redisUrl;
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
            builder.Services.AddOptions<BunnyOptions>()
                .BindConfiguration(nameof(BunnyOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            builder.Services.AddOptions<Common.Options.FileOptions>()
                .BindConfiguration(nameof(Common.Options.FileOptions))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddHangfire(config =>
            {
                var connectionString = BuildPostgresConnectionString(configuration);

                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(options =>
                        options.UseNpgsqlConnection(connectionString));
            });

            builder.Services.AddHangfireServer();

            builder.Services.AddHealthChecks()
                   .AddNpgSql(BuildPostgresConnectionString(configuration))
                   .AddHangfire(options => options.MinimumAvailableServers = 1)
                   .AddRedis(Environment.GetEnvironmentVariable("REDIS_URL")
                       ?? configuration.GetConnectionString("RedisConnection")!)
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
            builder.Services.AddScoped<IFileRepository, FileRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddHttpClient<IFileService, FileService>();
            builder.Services.AddScoped<ITenantMemberRepository, TenantMemberRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
        }
        public static string BuildPostgresConnectionString(IConfiguration configuration)
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (!string.IsNullOrWhiteSpace(databaseUrl))
            {
                var uri = new Uri(databaseUrl);
                var userInfo = uri.UserInfo.Split(':');

                return new NpgsqlConnectionStringBuilder
                {
                    Host = uri.Host,
                    Port = uri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = uri.AbsolutePath.Trim('/'),
                    SslMode = SslMode.Require
                }.ToString();
            }

            return configuration.GetConnectionString("DefaultConnection")!;
        }
    }
}