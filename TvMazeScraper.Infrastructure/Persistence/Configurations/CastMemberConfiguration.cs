using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvMazeScraper.Domain.CastMembers;

namespace TvMazeScraper.Infrastructure.Persistence.Configurations;

/// <summary>
///     Configurations for the <see cref="CastMember" /> entity
/// </summary>
public class CastMemberConfiguration : IEntityTypeConfiguration<CastMember>
{
    public void Configure(EntityTypeBuilder<CastMember> builder)
    {
        builder.ToTable("CastMembers");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasMaxLength(200);

        builder.Property(p => p.Birthday);
    }
}