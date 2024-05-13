using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvMazeScraper.Domain.JointTables;

namespace TvMazeScraper.Infrastructure.Persistence.Configurations;

/// <summary>
///     Configurations for the <see cref="CastMember" /> entity
/// </summary>
public class TvShowCastMemberConfiguration : IEntityTypeConfiguration<TvShowCastMember>
{
    public void Configure(EntityTypeBuilder<TvShowCastMember> builder)
    {
        builder.ToTable("TvShowCastMembers");

        builder.HasKey(tscm => new { tscm.TvShowId, tscm.CastMemberId });

        builder.Property(tscm => tscm.TvShowId)
            .IsRequired();

        builder.Property(tscm => tscm.CastMemberId)
            .IsRequired();
    }
}