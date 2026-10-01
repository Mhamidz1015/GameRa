using GameRa.Modules.Store.Domain.Wishlist;
using GameRa.Modules.Store.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Store.Infrastructure.Wishlists;

internal sealed class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
{
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.CustomerId).IsRequired();
        builder.Property(w => w.GameId).IsRequired();
        builder.Property(w => w.AddedAtUtc).IsRequired();

        builder.HasIndex(w => new { w.CustomerId, w.GameId }).IsUnique();

        builder.ToTable("wishlist_items", Schemas.Store);
    }
}