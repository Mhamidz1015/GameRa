using GameRa.Modules.Distribution.Domain.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Distribution.Infrastructure.Games;

internal sealed class DistributionGameConfiguration : IEntityTypeConfiguration<DistributionGame>
{
    public void Configure(EntityTypeBuilder<DistributionGame> builder)
    {
        builder.ToTable("distribution_games");

        builder.HasKey(g => g.GameId);

        builder.Property(g => g.GameId).ValueGeneratedNever();
    }
}
