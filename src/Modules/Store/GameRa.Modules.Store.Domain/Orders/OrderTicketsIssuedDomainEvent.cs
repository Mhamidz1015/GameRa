using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Orders;

public sealed class OrderTicketsIssuedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}
