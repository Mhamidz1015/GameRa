using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Wishlist.AddToWishlist;

public sealed record AddToWishlistCommand(Guid CustomerId, Guid GameId) : ICommand;