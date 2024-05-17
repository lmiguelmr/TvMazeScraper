using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Domain.TvShows;

namespace TvMazeScraper.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<TvShow> TvShows { get; set; } = null!;

    public DbSet<CastMember> CastMembers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<TvShow>()
                    .HasMany(e => e.CastMembers)
                    .WithMany();

        base.OnModelCreating(modelBuilder);
    }

    public async Task<IDbTransaction> BeginTranscationAsync()
    {
        return (await Database.BeginTransactionAsync()).GetDbTransaction();
    }
}
