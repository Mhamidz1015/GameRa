
using GameRa.Modules.Store.Domain.Wishlist;
using GameRa.Modules.Store.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Store.Infrastructure.Wishlists;

internal sealed class WishlistRepository(StoreDbContext context) : IWishlistRepository
{
    public async Task<bool> ExistsAsync(Guid customerId, Guid gameId, CancellationToken cancellationToken)
        => await context.Set<WishlistItem>()
            .AnyAsync(w => w.CustomerId == customerId && w.GameId == gameId, cancellationToken);

    public async Task<WishlistItem?> GetAsync(Guid customerId, Guid gameId, CancellationToken cancellationToken)
        => await context.Set<WishlistItem>()
            .FirstOrDefaultAsync(w => w.CustomerId == customerId && w.GameId == gameId, cancellationToken);

    public void Insert(WishlistItem item) => context.Set<WishlistItem>().Add(item);

    public void Remove(WishlistItem item) => context.Set<WishlistItem>().Remove(item);
}