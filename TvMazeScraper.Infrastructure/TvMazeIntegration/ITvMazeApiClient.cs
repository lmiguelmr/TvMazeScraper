using TvMazeScraper.Domain.TvShows;
using TvMazeScraper.Infrastructure.TvMazeIntegration.Entries;

namespace TvMazeScraper.Infrastructure.TvMazeIntegration;
public interface ITvMazeApiClient
{
    Task<IEnumerable<ShowEntry>> GetTvShowsAsync(int page = 0, CancellationToken cancellationToken = default);

    Task<IEnumerable<CastEntry>> GetCastAsync(int showId, CancellationToken cancellationToken = default);
}
