using GameRa.Modules.Distribution.Domain.Ownerships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Distribution.Infrastructure.Ownerships;

internal sealed class GameOwnershipConfiguration : IEntityTypeConfiguration<GameOwnership>
{
    public void Configure(EntityTypeBuilder<GameOwnership> builder)
    {
        builder.ToTable("game_ownerships");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.HasIndex(o => new { o.UserId, o.GameId }).IsUnique();
    }
}
