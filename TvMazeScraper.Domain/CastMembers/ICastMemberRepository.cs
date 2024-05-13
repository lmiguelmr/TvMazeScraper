using TvMazeScraper.Domain.TvShows;

namespace TvMazeScraper.Domain.CastMembers;

public interface ICastMemberRepository
{
    Task<IEnumerable<CastMember>> GetAllAsync(IEnumerable<int> castMemberIds, CancellationToken cancellationToken = default);

    Task AddNewCastMembersAsync(IEnumerable<CastMember> castMembers, CancellationToken cancellationToken = default);
}