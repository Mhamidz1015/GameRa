using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Games.integrationEvents;

public sealed class GameDelistedIntegrationEvent : IntegrationEvent
{
    public GameDelistedIntegrationEvent(Guid id, DateTime occurredOnUtc, Guid gameId)
        : base(id, occurredOnUtc)
    {
        GameId = gameId;
    }

    public Guid GameId { get; init; }
}
