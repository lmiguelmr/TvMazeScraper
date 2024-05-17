namespace TvMazeScraper.Domain.CastMembers;

public interface ICastMemberRepository
{
    Task<IEnumerable<CastMember>> GetAllByIdAsync(IEnumerable<int> castMemberIds, CancellationToken cancellationToken = default);

    Task<IEnumerable<int>> GetAllIdsAsync(IEnumerable<int> castMemberIds, CancellationToken cancellationToken = default);

    Task AddAsync(IEnumerable<CastMember> castMember, CancellationToken cancellationToken = default);
}