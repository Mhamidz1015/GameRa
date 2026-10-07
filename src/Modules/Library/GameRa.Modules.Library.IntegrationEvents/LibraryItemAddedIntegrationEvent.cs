using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Library.IntegrationEvents;

public sealed class LibraryItemAddedIntegrationEvent : IntegrationEvent
{
    public LibraryItemAddedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid userId,
        Guid gameId)
        : base(id, occurredOnUtc)
    {
        UserId = userId;
        GameId = gameId;
    }

    public Guid UserId { get; init; }

    public Guid GameId { get; init; }
}
