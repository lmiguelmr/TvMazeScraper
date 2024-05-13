using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Abstractions;
using TvMazeScraper.Domain.TvShows;
using TvMazeScraper.Infrastructure.Persistence;

namespace TvMazeScraper.Infrastructure.Repositories;

public class TvShowRepository : ITvShowRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _uowManager;

    public TvShowRepository(ApplicationDbContext context, IUnitOfWork uowManager)
    {
        _context = context;
        _uowManager = uowManager;
    }

    public async Task<int> GetTvShowCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TvShows.CountAsync(cancellationToken);
    }

    public async Task<TvShow> Add(TvShow tvShow)
    {
        var exists = await _context.TvShows.AnyAsync(x => x.Id == tvShow.Id);

        if (!exists)
        {
            await _context.TvShows.AddAsync(tvShow);

            if (!_uowManager.IsUnitOfWorkStarted)
                await _context.SaveChangesAsync();
        }

        return tvShow;
    }

    public async Task<IEnumerable<TvShow>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.TvShows
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}