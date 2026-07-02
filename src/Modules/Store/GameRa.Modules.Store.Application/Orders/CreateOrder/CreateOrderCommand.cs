using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Orders.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId) : ICommand;
