using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.Modules.Store.Application.Wishlist.GetWishlist
{
    public sealed class WishlistItemResponse
    {
        public Guid Id { get; init; }
        public Guid CustomerId { get; init; }
        public Guid GameId { get; init; }
        public DateTime AddedAtUtc { get; init; }
    }
}
