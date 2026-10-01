using GameRa.Modules.Library.Domain;
using GameRa.Modules.Library.Domain.LibraryItems;
using GameRa.Modules.Library.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Library.Infrastructure.Playtime;

internal sealed class PlaytimeRecordConfiguration : IEntityTypeConfiguration<PlaytimeRecord>
{
    public void Configure(EntityTypeBuilder<PlaytimeRecord> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.GameId).IsRequired();
        builder.Property(p => p.TotalMinutes).IsRequired();
        builder.Property(p => p.LastPlayedUtc).IsRequired();

        builder.HasIndex(p => new { p.UserId, p.GameId }).IsUnique();

        builder.ToTable("playtime_records", Schemas.LibraryItem);
    }
}