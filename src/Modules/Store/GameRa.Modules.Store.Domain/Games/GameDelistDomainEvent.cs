using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Games;

public sealed class GameDelistDomainEvent(Guid gameId) : DomainEvent
{
    public Guid GameId { get; } = gameId;
}
