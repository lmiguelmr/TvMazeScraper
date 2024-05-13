using Hangfire;

namespace TvMazeScraper.Infrastructure.TvMazeIntegration;

/// <summary>
/// ITvMazeJobs job interface.
/// </summary>
public interface ITvMazeJobs
{
    /// <summary>
    /// Import all pages/>.
    /// </summary>
    Task ImportAll(IJobCancellationToken cancellationToken);


    /// <summary>
    /// Update existing data/>.
    /// </summary>
    Task Update(IJobCancellationToken cancellationToken);
}
