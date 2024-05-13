namespace TvMazeScraper.Domain.JointTables;

public interface ITvShowCastMemberRepository
{
    Task<bool> ExistsAsync(int tvShowId, int castMemberId, CancellationToken cancellationToken = default);

    Task<IEnumerable<int>> GetCastIdsForTvShowAsync(int tvShowId, CancellationToken cancellationToken = default);

    Task AddRange(IEnumerable<TvShowCastMember> tvShowCastMembers, CancellationToken cancellationToken = default);
}