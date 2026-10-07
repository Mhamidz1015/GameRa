using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Distribution.IntegrationEvents;

public sealed class GameVersionPublishedIntegrationEvent : IntegrationEvent
{
    public GameVersionPublishedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid gameId,
        Guid versionId,
        string version)
        : base(id, occurredOnUtc)
    {
        GameId = gameId;
        VersionId = versionId;
        Version = version;
    }

    public Guid GameId { get; init; }

    public Guid VersionId { get; init; }

    public string Version { get; init; }
}
