using GameRa.Modules.Distribution.Domain.GameVersions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Distribution.Infrastructure.GameVersions;

internal sealed class VersionFileConfiguration : IEntityTypeConfiguration<VersionFile>
{
    public void Configure(EntityTypeBuilder<VersionFile> builder)
    {
        builder.ToTable("version_files");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id).ValueGeneratedNever();

        builder.Property(f => f.RelativePath).HasMaxLength(VersionFile.MaxPathLength);

        builder.Property(f => f.NormalizedPath).HasMaxLength(VersionFile.MaxPathLength);

        builder.Property(f => f.Sha256).HasMaxLength(64);

        builder.Property(f => f.StorageKey).HasMaxLength(200);

        builder.Property(f => f.UploadStatus).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(f => new { f.GameVersionId, f.NormalizedPath }).IsUnique();
    }
}
