using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Carts.RemoveItemFromCart;

public sealed record RemoveItemFromCartCommand(Guid CustomerId, Guid GameId) : ICommand;
