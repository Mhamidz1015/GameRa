using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Store.IntegrationEvents;

public sealed class OrderCompletedIntegrationEvent : IntegrationEvent
{
    public OrderCompletedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid customerId,
        Guid gameId,
        string gametitle)
        : base(id, occurredOnUtc)
    {
        CustomerId = customerId;
        GameId = gameId;
        GameTitle = gametitle;
    }

    public Guid CustomerId { get; init; }

    public Guid GameId { get; init; }

    public string GameTitle { get; init; }
}
