using GameRa.Modules.Reviews.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Reviews.Infrastructure.Reviews;

internal sealed class VerifiedPurchaseConfiguration : IEntityTypeConfiguration<VerifiedPurchase>
{
    public void Configure(EntityTypeBuilder<VerifiedPurchase> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.GameId).IsRequired();

        builder.Property(v => v.UserId).IsRequired();

        builder.Property(v => v.PurchasedAtUtc).IsRequired();

        builder.HasIndex(v => new { v.GameId, v.UserId })
            .IsUnique();
    }
}
