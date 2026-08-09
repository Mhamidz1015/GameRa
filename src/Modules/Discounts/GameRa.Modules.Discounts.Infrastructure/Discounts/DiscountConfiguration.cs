using GameRa.Modules.Discounts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Discounts.Infrastructure.Discounts;

internal sealed class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.DiscountId);

        builder.Property(d => d.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(d => d.Code)
            .IsUnique();

        builder.Property(d => d.Type)
            .IsRequired();

        builder.Property(d => d.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(d => d.Scope)
            .IsRequired();

        builder.Property(d => d.GameId);

        builder.Property(d => d.CategoryId);

        builder.Property(d => d.StartDateTimeUtc)
            .IsRequired();

        builder.Property(d => d.EndDateTimeUtc)
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired();
    }
}
