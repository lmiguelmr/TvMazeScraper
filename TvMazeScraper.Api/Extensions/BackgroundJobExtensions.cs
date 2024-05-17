using Hangfire;
using TvMazeScraper.Infrastructure.TvMazeIntegration;

namespace TvMazeScraper.Api.Extensions;

public static class BackgroundJobExtensions
{
    public static IApplicationBuilder UseBackGroundJobs(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<ITvMazeJobs>(
            $"{typeof(TvMazeJobs).Name}-ImportAll",
            x => x.ImportAll(null!),
            Cron.Never);

        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<ITvMazeJobs>(
            $"{typeof(TvMazeJobs).Name}-Update", 
            x => x.Update(null!),
            Cron.Never);

        return app;
    }
}

