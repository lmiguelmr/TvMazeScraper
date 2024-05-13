using TvMazeScraper.Domain.Abstractions;

namespace TvMazeScraper.Infrastructure.Persistence;

public class UnitOfWorkManager : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWorkManager(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool _isUnitOfWorkStarted = false;
    public void StartUnitOfWork()
    {
        _isUnitOfWorkStarted = true;
    }
    public bool IsUnitOfWorkStarted => _isUnitOfWorkStarted;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}