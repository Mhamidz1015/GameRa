using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Orders.GetOrdersByCustomerId;

public sealed record GetOrdersByCustomerIdQuery(Guid CustomerId)
    : IQuery<IReadOnlyCollection<OrderSummaryResponse>>;
