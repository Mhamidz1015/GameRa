using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Reviews.IntegrationEvents;

public sealed class ReviewUpdatedIntegrationEvent : IntegrationEvent
{
    public ReviewUpdatedIntegrationEvent(
        Guid id, DateTime occurredOnUtc,
        Guid reviewId, Guid gameId, int oldRating, int newRating)
        : base(id, occurredOnUtc)
    {
        ReviewId = reviewId;
        GameId = gameId;
        OldRating = oldRating;
        NewRating = newRating;
    }

    public Guid ReviewId { get; init; }
    public Guid GameId { get; init; }
    public int OldRating { get; init; }
    public int NewRating { get; init; }
}