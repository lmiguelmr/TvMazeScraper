using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.CastMembers;
using TvMazeScraper.Domain.JointTables;
using TvMazeScraper.Domain.TvShows;
using TvMazeScraper.Infrastructure.Persistence.Configurations;

namespace TvMazeScraper.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TvShow> TvShows { get; set; } = null!;
    public virtual DbSet<CastMember> CastMembers { get; set; } = null!;
    public virtual DbSet<TvShowCastMember> TvShowCastMembers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CastMemberConfiguration());
        modelBuilder.ApplyConfiguration(new TvShowCastMemberConfiguration());
        modelBuilder.ApplyConfiguration(new TvShowConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
