using GameRa.Modules.Distribution.Domain.GameVersions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Distribution.Infrastructure.GameVersions;

internal sealed class GameVersionConfiguration : IEntityTypeConfiguration<GameVersion>
{
    public void Configure(EntityTypeBuilder<GameVersion> builder)
    {
        builder.ToTable("game_versions");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id).ValueGeneratedNever();

        builder.Property(v => v.Status).HasConversion<string>().HasMaxLength(20);

        builder.Property(v => v.ReleaseNotes).HasMaxLength(GameVersion.MaxReleaseNotesLength);

        builder.HasIndex(v => new { v.GameId, v.Major, v.Minor, v.Patch }).IsUnique();

        builder.HasIndex(v => new { v.GameId, v.Status });

        builder.HasMany(v => v.Files)
            .WithOne()
            .HasForeignKey(f => f.GameVersionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.Files)
            .HasField("_files")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
