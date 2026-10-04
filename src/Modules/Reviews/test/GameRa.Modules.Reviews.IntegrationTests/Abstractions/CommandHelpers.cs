using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using GameRa.Modules.Reviews.Application.Reviews.DeleteReview;
using GameRa.Modules.Reviews.Application.Reviews.UpdateReview;

namespace GameRa.Modules.Reviews.IntegrationTests.Abstractions;

internal static class CommandHelpers
{
    internal static CreateReviewCommand CreateReview(
        Guid? gameId = null,
        Guid? userId = null,
        int rating = 4,
        string? comment = null)
    {
        return new CreateReviewCommand(
            gameId ?? Guid.NewGuid(),
            userId ?? Guid.NewGuid(),
            rating,
            comment ?? "Great game!");
    }

    internal static UpdateReviewCommand UpdateReview(
        Guid reviewId,
        Guid userId,
        int rating = 4,
        string? comment = null)
    {
        return new UpdateReviewCommand(
            reviewId,
            userId,
            rating,
            comment ?? "Updated comment");}

    internal static DeleteReviewCommand DeleteReview(
        Guid reviewId,
        Guid userId)
    {
        return new DeleteReviewCommand(reviewId, userId);
    }
}