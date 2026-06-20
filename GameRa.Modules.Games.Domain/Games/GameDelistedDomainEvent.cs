using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public sealed class GameDelistedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
