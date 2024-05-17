using Asp.Versioning;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TvMazeScraper.Application.Abstractions.Data;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Domain.TvShows;
using TvMazeScraper.Infrastructure.Data;
using TvMazeScraper.Infrastructure.Persistence;
using TvMazeScraper.Infrastructure.Repositories;
using TvMazeScraper.Infrastructure.TvMazeIntegration;

namespace TvMazeScraper.Infrastructure.Extensions;

/// <summary>
///     Extension methods for <see cref="IServiceCollection" />
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddApiVersioning(services);
        AddBackgroundJobs(services, configuration);

        return services;
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("Database") ??
                       throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
            options
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .UseSqlServer(connectionString, b => b.MigrationsAssembly("TvMazeScraper.Api"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWorkManager>();

        services.AddScoped<ICastMemberRepository, CastMemberRepository>();

        services.AddScoped<ITvShowRepository, TvShowRepository>();

        services.AddScoped<ITvMazeApiClient, TvMazeApiClient>();

        return services;
    }

    private static void AddApiVersioning(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHangfire(c => c
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("Database") ?? "", new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
            })
            .UseFilter(new AutomaticRetryAttribute { Attempts = 2 }));

        services.AddHangfireServer();

        services.AddScoped<ITvMazeJobs, TvMazeJobs>();
    }
}
