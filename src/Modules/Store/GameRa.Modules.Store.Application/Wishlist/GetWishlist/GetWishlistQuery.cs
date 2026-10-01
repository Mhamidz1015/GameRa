using GameRa.Common.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.Modules.Store.Application.Wishlist.GetWishlist
{
    public sealed record GetWishlistQuery(Guid CustomerId) : IQuery<IReadOnlyCollection<WishlistItemResponse>>;

}
