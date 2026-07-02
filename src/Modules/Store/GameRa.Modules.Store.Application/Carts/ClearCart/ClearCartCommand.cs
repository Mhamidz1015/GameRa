using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Carts.ClearCart;

public sealed record ClearCartCommand(Guid CustomerId) : ICommand;
