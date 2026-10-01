using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Reviews.Domain;

public sealed class ReviewUpdatedDomainEvent(
    Guid ReviewId,Guid GameId,int OldRating, int NewRating) : DomainEvent
{
    public Guid ReviewId { get; init; } = ReviewId;
    public Guid GameId { get; init; } = GameId;
    public int OldRating { get; init; } = OldRating;
    public int NewRating { get; init; } = NewRating;
}