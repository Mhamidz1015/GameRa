using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using GameRa.Modules.Reviews.Application.Reviews.DeleteReview;
using GameRa.Modules.Reviews.Application.Reviews.GetAverageRatingByGameId;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;
using GameRa.Modules.Reviews.Application.Reviews.GetReviewsByGameId;
using GameRa.Modules.Reviews.Application.Reviews.UpdateReview;
using GameRa.Modules.Reviews.Domain;
using GameRa.Modules.Reviews.IntegrationTests.Abstractions;

namespace GameRa.Modules.Reviews.IntegrationTests.Reviews;

public sealed class ReviewTests : BaseIntegrationTest
{
    public ReviewTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    // ─────────────────────────────────────────────
    // CreateReview
    // ─────────────────────────────────────────────

    [Fact]
    public async Task CreateReview_ShouldSucceed_WhenInputsAreValid()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateReview());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateReview_ShouldPersist_AndBeQueryable()
    {
        Guid gameId = Faker.Random.Guid();
        Guid userId = Faker.Random.Guid();
        const int rating = 5;
        string comment = Faker.Lorem.Sentence();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            gameId, userId, rating, comment));

        Result<ReviewResponse> getResult = await Sender.Send(
            new GetReviewQuery(createResult.Value));

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.GameId.Should().Be(gameId);
        getResult.Value.UserId.Should().Be(userId);
        getResult.Value.Rating.Should().Be(rating);
        getResult.Value.Comment.Should().Be(comment);
        getResult.Value.IsVerifiedPurchase.Should().BeFalse();
        getResult.Value.UpdatedAtUtc.Should().BeNull();
    }

    [Fact]
    public async Task CreateReview_ShouldFail_WhenRatingIsZero()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateReview(rating: 0));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateReview_ShouldFail_WhenRatingIsGreaterThanFive()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateReview(rating: 6));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateReview_ShouldFail_WhenCommentIsEmpty()
    {
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateReview(comment: string.Empty));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task CreateReview_ShouldFail_WhenUserAlreadyReviewedSameGame()
    {
        Guid gameId = Faker.Random.Guid();
        Guid userId = Faker.Random.Guid();

        await Sender.Send(CommandHelpers.CreateReview(gameId, userId, 4, Faker.Lorem.Sentence()));
        Result<Guid> result = await Sender.Send(CommandHelpers.CreateReview(gameId, userId, 5, Faker.Lorem.Sentence()));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.DuplicateReview(gameId));
    }

    [Fact]
    public async Task CreateReview_ShouldSucceed_WhenSameUserReviewsDifferentGames()
    {
        Guid userId = Faker.Random.Guid();

        Result<Guid> result1 = await Sender.Send(CommandHelpers.CreateReview(
            Faker.Random.Guid(), userId, 4, Faker.Lorem.Sentence()));

        Result<Guid> result2 = await Sender.Send(CommandHelpers.CreateReview(
            Faker.Random.Guid(), userId, 5, Faker.Lorem.Sentence()));

        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CreateReview_WithVerifiedPurchase_ShouldSetFlag()
    {
        Guid gameId = Faker.Random.Guid();
        Guid userId = Faker.Random.Guid();

        await SeedVerifiedPurchaseAsync(gameId, userId);

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            gameId, userId, 5, Faker.Lorem.Sentence()));

        Result<ReviewResponse> getResult = await Sender.Send(
            new GetReviewQuery(createResult.Value));

        getResult.Value.IsVerifiedPurchase.Should().BeTrue();
    }

    [Fact]
    public async Task CreateReview_WithoutVerifiedPurchase_ShouldNotSetFlag()
    {
        Guid gameId = Faker.Random.Guid();
        Guid userId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            gameId, userId, 5, Faker.Lorem.Sentence()));

        Result<ReviewResponse> getResult = await Sender.Send(
            new GetReviewQuery(createResult.Value));

        getResult.Value.IsVerifiedPurchase.Should().BeFalse();
    }

    // ─────────────────────────────────────────────
    // UpdateReview
    // ─────────────────────────────────────────────

    [Fact]
    public async Task UpdateReview_ShouldSucceed_WhenUserIsOwner()
    {
        Guid userId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            Faker.Random.Guid(), userId, 3, "Original comment"));

        Result updateResult = await Sender.Send(CommandHelpers.UpdateReview(
            createResult.Value, userId, 5, "Updated comment"));

        updateResult.IsSuccess.Should().BeTrue();

        Result<ReviewResponse> getResult = await Sender.Send(
            new GetReviewQuery(createResult.Value));

        getResult.Value.Rating.Should().Be(5);
        getResult.Value.Comment.Should().Be("Updated comment");
        getResult.Value.UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateReview_ShouldFail_WhenUserIsNotOwner()
    {
        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            Faker.Random.Guid(), Faker.Random.Guid(), 3, "Original"));

        Result updateResult = await Sender.Send(CommandHelpers.UpdateReview(
            createResult.Value, Faker.Random.Guid(), 5, "Updated"));

        updateResult.IsFailure.Should().BeTrue();
        updateResult.Error.Should().Be(ReviewErrors.Forbidden);
    }

    [Fact]
    public async Task UpdateReview_ShouldFail_WhenReviewDoesNotExist()
    {
        Result result = await Sender.Send(CommandHelpers.UpdateReview(
            Faker.Random.Guid(), Faker.Random.Guid(), 4, "Comment"));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateReview_ShouldFail_WhenRatingIsInvalid()
    {
        Guid userId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            Faker.Random.Guid(), userId, 3, "Original"));

        Result result = await Sender.Send(CommandHelpers.UpdateReview(
            createResult.Value, userId, 10, "Updated"));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateReview_ShouldNotChangeOwnership()
    {
        Guid userId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            gameId, userId, 3, "Original"));

        await Sender.Send(CommandHelpers.UpdateReview(
            createResult.Value, userId, 5, "Updated"));

        Result<ReviewResponse> getResult = await Sender.Send(
            new GetReviewQuery(createResult.Value));

        getResult.Value.UserId.Should().Be(userId);
        getResult.Value.GameId.Should().Be(gameId);
    }

    // ─────────────────────────────────────────────
    // DeleteReview
    // ─────────────────────────────────────────────

    [Fact]
    public async Task DeleteReview_ShouldSucceed_WhenUserIsOwner()
    {
        Guid userId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            Faker.Random.Guid(), userId, 4, Faker.Lorem.Sentence()));

        Result deleteResult = await Sender.Send(CommandHelpers.DeleteReview(
            createResult.Value, userId));

        deleteResult.IsSuccess.Should().BeTrue();

        Result<ReviewResponse> getResult = await Sender.Send(
            new GetReviewQuery(createResult.Value));

        getResult.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteReview_ShouldFail_WhenUserIsNotOwner()
    {
        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            Faker.Random.Guid(), Faker.Random.Guid(), 4, Faker.Lorem.Sentence()));

        Result deleteResult = await Sender.Send(CommandHelpers.DeleteReview(
            createResult.Value, Faker.Random.Guid()));

        deleteResult.IsFailure.Should().BeTrue();
        deleteResult.Error.Should().Be(ReviewErrors.Forbidden);
    }

    [Fact]
    public async Task DeleteReview_ShouldFail_WhenReviewDoesNotExist()
    {
        Result result = await Sender.Send(CommandHelpers.DeleteReview(
            Faker.Random.Guid(), Faker.Random.Guid()));

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteReview_ShouldAllowSameUser_ToReviewAgain_AfterDeletion()
    {
        Guid gameId = Faker.Random.Guid();
        Guid userId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            gameId, userId, 4, "First review"));

        await Sender.Send(CommandHelpers.DeleteReview(createResult.Value, userId));

        Result<Guid> secondResult = await Sender.Send(CommandHelpers.CreateReview(
            gameId, userId, 5, "Second review"));

        secondResult.IsSuccess.Should().BeTrue();
    }

    // ─────────────────────────────────────────────
    // GetReviewsByGameId
    // ─────────────────────────────────────────────

    [Fact]
    public async Task GetReviewsByGameId_ShouldReturnAllReviews_ForGame()
    {
        Guid gameId = Faker.Random.Guid();

        await Sender.Send(CommandHelpers.CreateReview(gameId, Faker.Random.Guid(), 5, "Excellent!"));
        await Sender.Send(CommandHelpers.CreateReview(gameId, Faker.Random.Guid(), 3, "Average"));
        await Sender.Send(CommandHelpers.CreateReview(gameId, Faker.Random.Guid(), 4, "Good"));

        Result<IReadOnlyCollection<ReviewResponse>> result = await Sender.Send(
            new GetReviewsByGameIdQuery(gameId));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Should().AllSatisfy(r => r.GameId.Should().Be(gameId));
    }

    [Fact]
    public async Task GetReviewsByGameId_ShouldReturnEmpty_WhenNoReviewsExist()
    {
        Result<IReadOnlyCollection<ReviewResponse>> result = await Sender.Send(
            new GetReviewsByGameIdQuery(Faker.Random.Guid()));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetReviewsByGameId_ShouldNotReturn_DeletedReviews()
    {
        Guid gameId = Faker.Random.Guid();
        Guid userId = Faker.Random.Guid();

        Result<Guid> createResult = await Sender.Send(CommandHelpers.CreateReview(
            gameId, userId, 4, "Will be deleted"));

        await Sender.Send(CommandHelpers.DeleteReview(createResult.Value, userId));

        Result<IReadOnlyCollection<ReviewResponse>> result = await Sender.Send(
            new GetReviewsByGameIdQuery(gameId));

        result.Value.Should().NotContain(r => r.ReviewId == createResult.Value);
    }

    [Fact]
    public async Task GetReviewsByGameId_ShouldNotReturn_ReviewsFromOtherGames()
    {
        Guid gameId = Faker.Random.Guid();
        Guid otherGameId = Faker.Random.Guid();

        await Sender.Send(CommandHelpers.CreateReview(gameId, Faker.Random.Guid(), 4, "My game"));
        await Sender.Send(CommandHelpers.CreateReview(otherGameId, Faker.Random.Guid(), 2, "Other game"));

        Result<IReadOnlyCollection<ReviewResponse>> result = await Sender.Send(
            new GetReviewsByGameIdQuery(gameId));

        result.Value.Should().AllSatisfy(r => r.GameId.Should().Be(gameId));
        result.Value.Should().NotContain(r => r.GameId == otherGameId);
    }

    // ─────────────────────────────────────────────
    // GetAverageRatingByGameId
    // ─────────────────────────────────────────────

    [Fact]
    public async Task GetAverageRating_ShouldReturnCorrectAverage()
    {
        Guid gameId = Faker.Random.Guid();

        await Sender.Send(CommandHelpers.CreateReview(gameId, Faker.Random.Guid(), 4, "Good"));
        await Sender.Send(CommandHelpers.CreateReview(gameId, Faker.Random.Guid(), 2, "Bad"));
        await Sender.Send(CommandHelpers.CreateReview(gameId, Faker.Random.Guid(), 3, "Okay"));

        Result<AverageRatingResponse> result = await Sender.Send(
            new GetAverageRatingByGameIdQuery(gameId));

        result.IsSuccess.Should().BeTrue();
        result.Value.AverageRating.Should().Be(3.0m);
        result.Value.TotalReviews.Should().Be(3);
        result.Value.GameId.Should().Be(gameId);
    }

    [Fact]
    public async Task GetAverageRating_ShouldReturnZero_WhenNoReviewsExist()
    {
        Result<AverageRatingResponse> result = await Sender.Send(
            new GetAverageRatingByGameIdQuery(Faker.Random.Guid()));

        result.IsSuccess.Should().BeTrue();
        result.Value.AverageRating.Should().Be(0);
        result.Value.TotalReviews.Should().Be(0);
    }

    [Fact]
    public async Task GetAverageRating_ShouldReturnCorrectValue_WithSingleReview()
    {
        Guid gameId = Faker.Random.Guid();

        await Sender.Send(CommandHelpers.CreateReview(
            gameId, Faker.Random.Guid(), 5, "Perfect"));

        Result<AverageRatingResponse> result = await Sender.Send(
            new GetAverageRatingByGameIdQuery(gameId));

        result.Value.AverageRating.Should().Be(5);
        result.Value.TotalReviews.Should().Be(1);
    }
}