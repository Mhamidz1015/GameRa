using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand(Guid CustomerId, Guid GameId) : ICommand;
