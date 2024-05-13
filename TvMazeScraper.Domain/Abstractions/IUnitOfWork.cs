namespace TvMazeScraper.Domain.Abstractions;

public interface IUnitOfWork
{
    void StartUnitOfWork();
    bool IsUnitOfWorkStarted { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}