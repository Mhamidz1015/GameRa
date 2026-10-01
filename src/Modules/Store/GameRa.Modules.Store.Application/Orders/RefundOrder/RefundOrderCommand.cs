using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Orders.RefundOrder;

public sealed record RefundOrderCommand(Guid OrderId, Guid CustomerId) : ICommand;
