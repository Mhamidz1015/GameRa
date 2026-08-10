using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.UnitTests.Abstractions;

namespace GameRa.Modules.Reviews.UnitTests.Reviews;

public class ReviewTests : BaseTest
{
    // ─────────────────────────────────────────────
    // Create — Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Create_ShouldReturnFailure_WhenRatingIsZero()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            0,
            Faker.Lorem.Sentence(),
            false);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.InvalidRating);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenRatingIsGreaterThanFive()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            6,
            Faker.Lorem.Sentence(),
            false);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.InvalidRating);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenRatingIsNegative()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            -1,
            Faker.Lorem.Sentence(),
            false);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.InvalidRating);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenCommentIsEmpty()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            4,
            string.Empty,
            false);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.CommentRequired);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenCommentIsWhitespace()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            4,
            "   ",
            false);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.CommentRequired);
    }

    // ─────────────────────────────────────────────
    // Create — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Create_ShouldSucceed_WithValidInputs()
    {
        Guid gameId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        const int rating = 4;
        string comment = Faker.Lorem.Sentence();

        Result<Review> result = Review.Create(gameId, userId, rating, comment, false);

        result.IsSuccess.Should().BeTrue();
        result.Value.GameId.Should().Be(gameId);
        result.Value.UserId.Should().Be(userId);
        result.Value.Rating.Should().Be(rating);
        result.Value.Comment.Should().Be(comment);
        result.Value.IsVerifiedPurchase.Should().BeFalse();
        result.Value.UpdatedAtUtc.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldSucceed_WithVerifiedPurchase()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(), Guid.NewGuid(), 5,
            Faker.Lorem.Sentence(), true);

        result.IsSuccess.Should().BeTrue();
        result.Value.IsVerifiedPurchase.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Create_ShouldSucceed_WithAllValidRatings(int rating)
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(), Guid.NewGuid(), rating,
            Faker.Lorem.Sentence(), false);

        result.IsSuccess.Should().BeTrue();
        result.Value.Rating.Should().Be(rating);
    }

    [Fact]
    public void Create_ShouldRaiseReviewCreatedDomainEvent()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(), Guid.NewGuid(), 4,
            Faker.Lorem.Sentence(), false);

        ReviewCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<ReviewCreatedDomainEvent>(result.Value);

        domainEvent.ReviewId.Should().Be(result.Value.ReviewId);
    }

    [Fact]
    public void Create_ShouldSetCreatedAtUtc()
    {
        DateTime before = DateTime.UtcNow;

        Result<Review> result = Review.Create(
            Guid.NewGuid(), Guid.NewGuid(), 4,
            Faker.Lorem.Sentence(), false);

        DateTime after = DateTime.UtcNow;

        result.Value.CreatedAtUtc.Should().BeOnOrAfter(before);
        result.Value.CreatedAtUtc.Should().BeOnOrBefore(after);
    }

    // ─────────────────────────────────────────────
    // Update — Failure Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Update_ShouldReturnFailure_WhenRatingIsZero()
    {
        Review review = CreateDefaultReview();

        Result result = review.Update(0, Faker.Lorem.Sentence());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.InvalidRating);
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenRatingIsGreaterThanFive()
    {
        Review review = CreateDefaultReview();

        Result result = review.Update(6, Faker.Lorem.Sentence());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.InvalidRating);
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenCommentIsEmpty()
    {
        Review review = CreateDefaultReview();

        Result result = review.Update(4, string.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.CommentRequired);
    }

    [Fact]
    public void Update_ShouldReturnFailure_WhenCommentIsWhitespace()
    {
        Review review = CreateDefaultReview();

        Result result = review.Update(4, "   ");

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.CommentRequired);
    }

    // ─────────────────────────────────────────────
    // Update — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Update_ShouldSucceed_WithValidInputs()
    {
        Review review = CreateDefaultReview();
        const int newRating = 2;
        string newComment = "Updated comment";

        Result result = review.Update(newRating, newComment);

        result.IsSuccess.Should().BeTrue();
        review.Rating.Should().Be(newRating);
        review.Comment.Should().Be(newComment);
    }

    [Fact]
    public void Update_ShouldSetUpdatedAtUtc_WhenSuccessful()
    {
        Review review = CreateDefaultReview();
        DateTime before = DateTime.UtcNow;

        review.Update(3, "New comment");

        DateTime after = DateTime.UtcNow;

        review.UpdatedAtUtc.Should().NotBeNull();
        review.UpdatedAtUtc.Should().BeOnOrAfter(before);
        review.UpdatedAtUtc.Should().BeOnOrBefore(after);
    }

    [Fact]
    public void Update_ShouldRaiseReviewUpdatedDomainEvent_WhenSuccessful()
    {
        Review review = CreateDefaultReview();
        review.ClearDomainEvents();

        review.Update(3, "Updated");

        ReviewUpdatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<ReviewUpdatedDomainEvent>(review);

        domainEvent.ReviewId.Should().Be(review.ReviewId);
    }

    [Fact]
    public void Update_ShouldNotChangeGameId_WhenSuccessful()
    {
        Guid originalGameId = Guid.NewGuid();
        Review review = Review.Create(originalGameId, Guid.NewGuid(), 4,
            Faker.Lorem.Sentence(), false).Value;

        review.Update(5, "New comment");

        review.GameId.Should().Be(originalGameId);
    }

    [Fact]
    public void Update_ShouldNotChangeUserId_WhenSuccessful()
    {
        Guid originalUserId = Guid.NewGuid();
        Review review = Review.Create(Guid.NewGuid(), originalUserId, 4,
            Faker.Lorem.Sentence(), false).Value;

        review.Update(5, "New comment");

        review.UserId.Should().Be(originalUserId);
    }

    [Fact]
    public void Update_ShouldNotChangeIsVerifiedPurchase_WhenSuccessful()
    {
        Review review = Review.Create(Guid.NewGuid(), Guid.NewGuid(), 4,
            Faker.Lorem.Sentence(), true).Value;

        review.Update(3, "New comment");

        review.IsVerifiedPurchase.Should().BeTrue();
    }

    // ─────────────────────────────────────────────
    // Delete — Success Cases
    // ─────────────────────────────────────────────

    [Fact]
    public void Delete_ShouldRaiseReviewDeletedDomainEvent()
    {
        Review review = CreateDefaultReview();
        review.ClearDomainEvents();

        review.Delete();

        ReviewDeletedDomainEvent domainEvent =
            AssertDomainEventWasPublished<ReviewDeletedDomainEvent>(review);

        domainEvent.ReviewId.Should().Be(review.ReviewId);
    }

    [Fact]
    public void Delete_ShouldRaiseOnlyOneEvent()
    {
        Review review = CreateDefaultReview();
        review.ClearDomainEvents();

        review.Delete();

        review.DomainEvents.Should().HaveCount(1);
        review.DomainEvents.Single().Should().BeOfType<ReviewDeletedDomainEvent>();
    }

    // ─────────────────────────────────────────────
    // VerifiedPurchase Tests
    // ─────────────────────────────────────────────

    [Fact]
    public void Create_ShouldSetIsVerifiedPurchaseFalse_ByDefault()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(), Guid.NewGuid(), 4,
            Faker.Lorem.Sentence(), false);

        result.Value.IsVerifiedPurchase.Should().BeFalse();
    }

    [Fact]
    public void Create_ShouldSetIsVerifiedPurchaseTrue_WhenVerified()
    {
        Result<Review> result = Review.Create(
            Guid.NewGuid(), Guid.NewGuid(), 4,
            Faker.Lorem.Sentence(), true);

        result.Value.IsVerifiedPurchase.Should().BeTrue();
    }

    // ─────────────────────────────────────────────
    // VerifiedPurchase Entity Tests
    // ─────────────────────────────────────────────

    [Fact]
    public void VerifiedPurchase_Create_ShouldSucceed_WithValidInputs()
    {
        Guid gameId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        DateTime purchasedAt = DateTime.UtcNow;

        VerifiedPurchase vp = VerifiedPurchase.Create(gameId, userId, purchasedAt);

        vp.GameId.Should().Be(gameId);
        vp.UserId.Should().Be(userId);
        vp.PurchasedAtUtc.Should().Be(purchasedAt);
        vp.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void VerifiedPurchase_Create_ShouldGenerateUniqueIds()
    {
        VerifiedPurchase vp1 = VerifiedPurchase.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        VerifiedPurchase vp2 = VerifiedPurchase.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        vp1.Id.Should().NotBe(vp2.Id);
    }

    // ─────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────

    private static Review CreateDefaultReview() =>
        Review.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            4,
            "Great game, highly recommended!",
            false).Value;
}
