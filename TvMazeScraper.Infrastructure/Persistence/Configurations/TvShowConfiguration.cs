using TvMazeScraper.Domain.TvShows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TvMazeScraper.Infrastructure.Persistence.Configurations;

/// <summary>
///     Configurations for the <see cref="TvShow" /> entity
/// </summary>
public class TvShowConfiguration : IEntityTypeConfiguration<TvShow>
{
    public void Configure(EntityTypeBuilder<TvShow> builder)
    {
        builder.ToTable("TvShows");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasMaxLength(200);
    }
}