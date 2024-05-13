using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Infrastructure.Persistence;

namespace TvMazeScraper.Infrastructure.Repositories;

internal sealed class CastMemberRepository : ICastMemberRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _uowManager;

    public CastMemberRepository(ApplicationDbContext context, IUnitOfWork uowManager)
    {
        _context = context;
        _uowManager = uowManager;
    }

    public async Task<IEnumerable<CastMember>> GetAllAsync(IEnumerable<int> castMemberIds, CancellationToken cancellationToken = default)
    {
        return await _context.CastMembers
            .Where(castMember => castMemberIds.Contains(castMember.Id))
            .OrderBy(castMember => castMember.Birthday)
            .ToListAsync(cancellationToken);
    }

    public async Task AddNewCastMembersAsync(IEnumerable<CastMember> castMembers, CancellationToken cancellationToken = default)
    {
        var existingCastMemberIds = await _context.CastMembers
            .Where(castMember => castMembers.Select(c => c.Id).Contains(castMember.Id))
            .Select(castMember => castMember.Id)
            .ToListAsync(cancellationToken);

        // Filter out the cast members that already exist in the database
        var newCastMembers = castMembers
            .Where(c => !existingCastMemberIds.Contains(c.Id))
            .ToList();

        // If there are new cast members to add, retrieve their details and add them to the database
        if (newCastMembers.Count != 0)
        {
            await _context.CastMembers.AddRangeAsync(newCastMembers, cancellationToken);

            if (!_uowManager.IsUnitOfWorkStarted)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}