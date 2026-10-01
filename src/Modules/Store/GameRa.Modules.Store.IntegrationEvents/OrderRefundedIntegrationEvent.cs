using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Store.IntegrationEvents;

public sealed class OrderRefundedIntegrationEvent : IntegrationEvent
{
    public OrderRefundedIntegrationEvent(
        Guid id, DateTime occurredOnUtc,
        Guid orderId, Guid customerId,
        List<OrderCompletedGameModel> games)
        : base(id, occurredOnUtc)
    {
        OrderId = orderId;
        CustomerId = customerId;
        Games = games;
    }

    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public List<OrderCompletedGameModel> Games { get; init; }
}