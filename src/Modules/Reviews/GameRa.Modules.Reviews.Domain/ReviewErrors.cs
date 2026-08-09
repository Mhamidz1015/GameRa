using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Reviews.Domain;

public static class ReviewErrors
{
    public static Error NotFound(Guid reviewId) =>
        Error.NotFound("Review.NotFound", $"The review with the specified identifier {reviewId} was not found.");

    public static readonly Error InvalidRating = Error.Problem(
        "Review.InvalidRating",
        "The rating must be between 1 and 5.");

    public static readonly Error CommentRequired = Error.Problem(
        "Review.CommentRequired",
        "The review comment cannot be empty.");

    public static Error DuplicateReview(Guid gameId) => Error.Conflict(
        "Review.DuplicateReview",
        $"The user has already submitted a review for game {gameId}.");

    public static readonly Error Forbidden = Error.Problem(
        "Review.Forbidden",
        "You are not allowed to modify or delete this review.");
}