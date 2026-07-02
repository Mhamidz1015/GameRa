using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Orders;

public sealed class OrderCreatedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}
