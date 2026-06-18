using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public sealed class GameCreatedDomainEvent(Guid GameId) : DomainEvent
{
    public Guid GameId { get; init; } = GameId;
}
