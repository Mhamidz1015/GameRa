using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Store.Domain.Games;

public sealed class GamePaymentsRefundedDomainEvent(Guid gameId) : DomainEvent
{
    public Guid GameId { get; init; } = gameId;
}
