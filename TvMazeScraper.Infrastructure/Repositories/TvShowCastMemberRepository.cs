using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.JointTables;
using TvMazeScraper.Infrastructure.Persistence;

namespace TvMazeScraper.Infrastructure.Repositories;

public class TvShowCastMemberRepository : ITvShowCastMemberRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _uowManager;

    public TvShowCastMemberRepository(ApplicationDbContext context, IUnitOfWork uowManager)
    {
        _context = context;
        _uowManager = uowManager;
    }

    public async Task<IEnumerable<int>> GetCastIdsForTvShowAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        return await _context.TvShowCastMembers
            .Where(tc => tc.TvShowId == tvShowId)
            .Select(tc => tc.CastMemberId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int tvShowId, int castMemberId, CancellationToken cancellationToken = default)
    {
        return await _context
            .TvShowCastMembers
            .AnyAsync(castMember => castMember.TvShowId == tvShowId && castMember.CastMemberId == castMemberId, cancellationToken);
    }

    public async Task AddRange(IEnumerable<TvShowCastMember> tvShowCastMembers, CancellationToken cancellationToken = default)
    {
        await _context.TvShowCastMembers.AddRangeAsync(tvShowCastMembers, cancellationToken);

        if (!_uowManager.IsUnitOfWorkStarted)
            await _context.SaveChangesAsync(cancellationToken);
    }
}