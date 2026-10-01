using GameRa.Modules.Store.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.Modules.Store.Domain.Wishlist
{
    public interface IWishlistRepository
    {
        Task<WishlistItem?> GetAsync(Guid customerId, Guid gameId, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid customerId, Guid GameId, CancellationToken cancellationToken = default);

        void Insert(WishlistItem item);
        void Remove(WishlistItem item);
    }
}
