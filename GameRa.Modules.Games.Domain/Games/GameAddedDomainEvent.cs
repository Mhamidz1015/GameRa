using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public sealed class GameAddedDomainEvent(Guid GameId) : DomainEvent
{
    public Guid GameId { get; init; } = GameId;
}
