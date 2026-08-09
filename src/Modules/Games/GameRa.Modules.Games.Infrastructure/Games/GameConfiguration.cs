using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.Domain.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Games.Infrastructure.Games;

internal sealed class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title).HasMaxLength(200).IsRequired();
        builder.Property(g => g.Description).HasMaxLength(2000).IsRequired();
        builder.Property(g => g.Developer).HasMaxLength(200).IsRequired();
        builder.Property(g => g.BasePrice).HasPrecision(18, 2).IsRequired();
        builder.Property(g => g.CoverImageUrl).HasMaxLength(500).IsRequired();
        builder.Property(g => g.Status).IsRequired();
        builder.Property(g => g.CategoryId).IsRequired();

        // Read model fields
        builder.Property(g => g.ActiveDiscountAmount).HasPrecision(18, 2);
        builder.Property(g => g.IsDiscountPercentage);
        builder.Property(g => g.AverageRating);
        builder.Property(g => g.TotalReviews);


        builder.Ignore(g => g.CurrentPrice);

        builder.HasOne<Category>().WithMany();
    }
}
