using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Orders.GetOrders;

public sealed record GetOrdersQuery(Guid CustomerId) : IQuery<IReadOnlyCollection<OrderResponse>>;
