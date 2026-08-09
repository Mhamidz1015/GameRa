using GameRa.Common.Application.MessagingEventBus;

namespace GameRa.Modules.Reviews.IntegrationEvents;

public sealed class ReviewCreatedIntegrationEvent : IntegrationEvent
{
    public ReviewCreatedIntegrationEvent(
        Guid id,
        DateTime occurredOnUtc,
        Guid reviewId,
        Guid gameId,
        Guid userId,
        int rating,
        bool isVerifiedPurchase)
        : base(id, occurredOnUtc)
    {
        ReviewId = reviewId;
        GameId = gameId;
        UserId = userId;
        Rating = rating;
        IsVerifiedPurchase = isVerifiedPurchase;
    }

    public Guid ReviewId { get; init; }

    public Guid GameId { get; init; }

    public Guid UserId { get; init; }

    public int Rating { get; init; }

    public bool IsVerifiedPurchase { get; init; }
}
