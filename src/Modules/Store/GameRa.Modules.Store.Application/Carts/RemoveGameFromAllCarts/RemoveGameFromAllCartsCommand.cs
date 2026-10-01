using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Carts.RemoveGameFromAllCarts;

public sealed record RemoveGameFromAllCartsCommand(Guid GameId) : ICommand;