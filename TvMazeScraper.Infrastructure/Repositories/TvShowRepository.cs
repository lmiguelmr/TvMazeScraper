using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.TvShows;
using TvMazeScraper.Infrastructure.Persistence;

namespace TvMazeScraper.Infrastructure.Repositories;

public class TvShowRepository : ITvShowRepository
{
    private readonly ApplicationDbContext _context;

    public TvShowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TvShow?> GetById(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TvShows
            .FindAsync(id);
    }

    public async Task<int> GetTvShowCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TvShows.CountAsync(cancellationToken);
    }

    public async Task<TvShow> AddAsync(TvShow tvShow, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(tvShow, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return tvShow;
    }

    public async Task AddRelation(int tvShowId, IEnumerable<int> castMembersIds, CancellationToken cancellationToken = default)
    {
        // Fetch the TV show with its current cast members and the relevant cast members in a single query
        var tvShowWithCast = await _context.TvShows
            .Where(t => t.Id == tvShowId)
            .Select(t => new
            {
                TvShow = t,
                CurrentCastIds = t.CastMembers.Select(cm => cm.Id).ToList(),
                NewCastMembers = _context.CastMembers.Where(cm => castMembersIds.Contains(cm.Id)).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("TV Show not found");

        // Filter out already existing cast members to avoid adding duplicates
        var newCastMembers = tvShowWithCast.NewCastMembers
            .Where(cm => !tvShowWithCast.CurrentCastIds.Contains(cm.Id))
            .ToList();

        if (newCastMembers.Count != 0)
        {
            tvShowWithCast.TvShow.CastMembers.AddRange(newCastMembers);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task AddMissingCastMembersRelation(int tvSowId, IEnumerable<int> castMembersIds, CancellationToken cancellationToken = default)
    {
        var tvShow = _context.TvShows
            .Include(c => c.CastMembers)
            .FirstOrDefault(t => t.Id == tvSowId);

        var newCast = tvShow.CastMembers.Where(c => !castMembersIds.Contains(c.Id));

        tvShow.CastMembers.AddRange(newCast);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TvShow>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.TvShows
            .AsNoTracking()
            .Include(c => c.CastMembers)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<TvShow> tvShows, int totalCount)> GetAllAsync2(int page, int pageSize, CancellationToken cancellationToken)
    {
        var pageTvShows = await _context.TvShows
            .Include(c => c.CastMembers)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await _context.TvShows.CountAsync();

        return (pageTvShows, totalCount);
    }
}