using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Reviews.Domain;

public sealed class Review : Entity
{
    private Review()
    {
    }

    public Guid ReviewId { get; private set; }

    public Guid GameId { get; private set; }

    public Guid UserId { get; private set; }

    public int Rating { get; private set; }

    public string Comment { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public bool IsVerifiedPurchase { get; private set; }

    public static Result<Review> Create(
        Guid gameId,
        Guid userId,
        int rating,
        string comment,
        bool isVerifiedPurchase)
    {
        if (rating is < 1 or > 5)
        {
            return Result.Failure<Review>(ReviewErrors.InvalidRating);
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            return Result.Failure<Review>(ReviewErrors.CommentRequired);
        }

        var review = new Review
        {
            ReviewId = Guid.NewGuid(),
            GameId = gameId,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            IsVerifiedPurchase = isVerifiedPurchase,
            CreatedAtUtc = DateTime.UtcNow
        };

        review.Raise(new ReviewCreatedDomainEvent(review.ReviewId));

        return Result.Success(review);
    }

    public Result Update(int rating, string comment)
    {
        if (rating is < 1 or > 5)
        {
            return Result.Failure(ReviewErrors.InvalidRating);
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            return Result.Failure(ReviewErrors.CommentRequired);
        }

        Rating = rating;
        Comment = comment;
        UpdatedAtUtc = DateTime.UtcNow;

        Raise(new ReviewUpdatedDomainEvent(ReviewId));

        return Result.Success();
    }

    public void Delete()
    {
        Raise(new ReviewDeletedDomainEvent(ReviewId, GameId, Rating));
    }
}