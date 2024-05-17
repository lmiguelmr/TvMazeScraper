namespace TvMazeScraper.Domain.TvShows;
public interface ITvShowRepository
{
    Task<TvShow?> GetById(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TvShow>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<(IEnumerable<TvShow> tvShows, int totalCount)> GetAllAsync2(int page, int pageSize, CancellationToken cancellationToken);

    Task<TvShow> AddAsync(TvShow tvshow, CancellationToken cancellationToken = default);

    Task AddRelation(int tvShowId, IEnumerable<int>? castMembersIds, CancellationToken cancellationToken = default);

    Task AddMissingCastMembersRelation(int tvSowId, IEnumerable<int> castMembersIds, CancellationToken cancellationToken = default);

}
