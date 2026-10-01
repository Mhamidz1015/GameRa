using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Orders;

public sealed class OrderRefundedDomainEvent(
    Guid OrderId,
    Guid CustomerId,
    List<Guid> GameIds) : DomainEvent
{
    public Guid OrderId { get; init; } = OrderId;
    public Guid CustomerId { get; init; } = CustomerId;
    public List<Guid> GameIds { get; init; } = GameIds;
}