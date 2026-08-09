using GameRa.Modules.Reviews.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Reviews.Infrastructure.Reviews;

internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.ReviewId);

        builder.Property(r => r.GameId).IsRequired();

        builder.Property(r => r.UserId).IsRequired();

        builder.Property(r => r.Rating).IsRequired();

        builder.Property(r => r.Comment)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(r => r.IsVerifiedPurchase).IsRequired();

        builder.Property(r => r.CreatedAtUtc).IsRequired();

        builder.Property(r => r.UpdatedAtUtc);

        builder.HasIndex(r => new { r.GameId, r.UserId })
            .IsUnique();
    }
}
