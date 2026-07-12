using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Games.IntegrationEvents;

public sealed class GameDelistedIntegrationEvent : IntegrationEvent
{
    public GameDelistedIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid gameId)
        : base(id, occurredOnUtc)
    {
        GameId = gameId;
    }

    public Guid GameId { get; init; }
}
