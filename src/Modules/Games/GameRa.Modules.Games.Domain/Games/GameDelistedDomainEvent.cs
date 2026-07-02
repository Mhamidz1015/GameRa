using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public sealed class GameDelistedDomainEvent(Guid gameId) : DomainEvent
{
    public Guid GameId { get; init; } = gameId;
}
