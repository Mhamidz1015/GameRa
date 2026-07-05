using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Store.integrationEvents;

public sealed class GameArchivedIntegrationEvent : IntegrationEvent
{
    public GameArchivedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid gameId,
        string code)
        : base(id, occurredOnUtc)
    {
        GameId = gameId;
        Code = code;
    }

    public Guid GameId { get; init; }

    public string Code { get; init; }
}
