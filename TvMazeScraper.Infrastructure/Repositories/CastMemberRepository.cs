using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Infrastructure.Persistence;

namespace TvMazeScraper.Infrastructure.Repositories;

internal sealed class CastMemberRepository : ICastMemberRepository
{
    private readonly ApplicationDbContext _context;

    public CastMemberRepository(ApplicationDbContext context, IUnitOfWork uowManager)
    {
        _context = context;
    }

    public async Task<IEnumerable<CastMember>> GetAllByIdAsync(IEnumerable<int> castMemberIds, CancellationToken cancellationToken = default)
    {
        return await _context.CastMembers
            .Where(c => castMemberIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<int>> GetAllIdsAsync(IEnumerable<int> castMemberIds, CancellationToken cancellationToken = default)
    {
        return await _context.CastMembers
            .Where(castMember => castMemberIds.Contains(castMember.Id))
            .Select(castMember => castMember.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(IEnumerable<CastMember> castMember, CancellationToken cancellationToken = default)
    {
        await _context.CastMembers
            .AddRangeAsync(castMember, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
