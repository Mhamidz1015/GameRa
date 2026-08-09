using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Reviews.Application.Reviews.UpdateReview;

public sealed record UpdateReviewCommand(
    Guid ReviewId,
    Guid UserId,
    int Rating,
    string Comment) : ICommand;