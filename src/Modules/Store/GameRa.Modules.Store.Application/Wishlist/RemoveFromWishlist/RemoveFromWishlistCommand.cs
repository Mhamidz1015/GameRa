using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Wishlist.RemoveFromWishlist;

public sealed record RemoveFromWishlistCommand(Guid CustomerId, Guid GameId) : ICommand;