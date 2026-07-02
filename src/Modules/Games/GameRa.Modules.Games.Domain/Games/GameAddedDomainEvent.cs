using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Games.Domain.Games;

public sealed class GameAddedDomainEvent(Guid GameId) : DomainEvent
{
    public Guid GameId { get; init; } = GameId;
}
