namespace TvMazeScraper.Domain.TvShows;
public interface ITvShowRepository
{
    Task<IEnumerable<TvShow?>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<int> GetTvShowCountAsync(CancellationToken cancellationToken = default);

    Task<TvShow> Add(TvShow tvshow);
}
