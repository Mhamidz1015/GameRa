using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Reviews.IntegrationEvents;

public sealed class ReviewDeletedIntegrationEvent : IntegrationEvent
{
    public ReviewDeletedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid reviewId,
        Guid gameId,
        int rating)
        : base(id, occurredOnUtc)
    {
        ReviewId = reviewId;
        GameId = gameId;
        Rating = rating;
    }

    public Guid ReviewId { get; init; }

    public Guid GameId { get; init; }

    public int Rating { get; init; }
}
