using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Reviews.Domain;

public sealed class ReviewDeletedDomainEvent(Guid ReviewId, Guid GameId, int Rating) : DomainEvent
{
    public Guid ReviewId { get; init; } = ReviewId;

    public Guid GameId { get; init; } = GameId;

    public int Rating { get; init; } = Rating;
}