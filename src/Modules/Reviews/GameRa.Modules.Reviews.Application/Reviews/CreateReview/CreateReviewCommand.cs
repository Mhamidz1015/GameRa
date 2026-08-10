using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Reviews.Application.Reviews.CreateReview;

public sealed record CreateReviewCommand(
    Guid GameId,
    Guid UserId,
    int Rating,
    string Comment,
    bool verifiedPurchase) : ICommand<Guid>;