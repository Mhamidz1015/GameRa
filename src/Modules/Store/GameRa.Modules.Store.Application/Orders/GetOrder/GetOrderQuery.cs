using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Orders.GetOrder;

public sealed record GetOrderQuery(Guid OrderId) : IQuery<OrderResponse>;
