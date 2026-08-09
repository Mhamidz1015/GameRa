using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Store.IntegrationEvents;

public sealed class OrderCompletedIntegrationEvent : IntegrationEvent
{
    public OrderCompletedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid customerId,
        List<OrderCompletedGameModel> games)
        : base(id, occurredOnUtc)
    {
        CustomerId = customerId;
        Games = games;
    }

    public Guid CustomerId { get; init; }
    public List<OrderCompletedGameModel> Games { get; init; }
}
