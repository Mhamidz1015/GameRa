namespace GameRa.Modules.Reviews.Application.Reviews.GetReview;

public sealed record ReviewResponse(
    Guid ReviewId,
    Guid GameId,
    Guid UserId,
    int Rating,
    string Comment,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsVerifiedPurchase);