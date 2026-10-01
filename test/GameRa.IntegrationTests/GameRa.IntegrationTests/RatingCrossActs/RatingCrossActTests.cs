using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Games.Application.Games.GetGame;
using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using GameRa.Modules.Reviews.Application.Reviews.DeleteReview;
using GameRa.Modules.Reviews.Application.Reviews.UpdateReview;

namespace GameRa.IntegrationTests.RatingCrossActs;

public sealed class RatingCrossActTests : BaseIntegrationTest
{
    public RatingCrossActTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task ReviewCreated_ShouldUpdateGameAverageRating()
    {
        // Arrange
        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);

        // Act
        await Sender.Send(new CreateReviewCommand(gameId, Faker.Random.Guid(), 4, "Good game"));

        // Poll تا rating آپدیت بشه
        Result<GameResponse> gameResult = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<GameResponse?> g = await Sender.Send(new GetGameQuery(gameId));

            if (g.IsFailure || g.Value is null || g.Value.TotalReviews < 1)
                return Result.Failure<GameResponse>(
                    Error.Failure("Rating.NotUpdated", "Rating not updated yet"));

            return Result.Success(g.Value);
        });

        // Assert
        gameResult.IsSuccess.Should().BeTrue();
        gameResult.Value.AverageRating.Should().Be(4);
        gameResult.Value.TotalReviews.Should().Be(1);
    }

    [Fact]
    public async Task ReviewDeleted_ShouldUpdateGameAverageRating()
    {
        // Arrange
        Guid gameId = Faker.Random.Guid();
        Guid userId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);

        Result<Guid> reviewResult = await Sender.Send(
            new CreateReviewCommand(gameId, userId, 5, "Amazing"));

        // Wait for rating update
        await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<GameResponse?> g = await Sender.Send(new GetGameQuery(gameId));
            if (g.Value?.TotalReviews < 1)
                return Result.Failure<GameResponse?>(Error.Failure("x", "x"));
            return g;
        });

        // Act — delete review
        await Sender.Send(new DeleteReviewCommand(reviewResult.Value, userId));

        // Poll تا rating به صفر برگرده
        Result<GameResponse> afterDelete = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<GameResponse?> g = await Sender.Send(new GetGameQuery(gameId));

            if (g.IsFailure || g.Value?.TotalReviews != 0)
                return Result.Failure<GameResponse>(
                    Error.Failure("Rating.NotUpdated", "Rating not updated yet"));

            return Result.Success(g.Value);
        });

        // Assert
        afterDelete.IsSuccess.Should().BeTrue();
        afterDelete.Value.TotalReviews.Should().Be(0);
        afterDelete.Value.AverageRating.Should().Be(0);
    }

    [Fact]
    public async Task MultipleReviews_ShouldCalculateCorrectAverage()
    {
        // Arrange
        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);

        await Sender.Send(new CreateReviewCommand(gameId, Faker.Random.Guid(), 4, "Good"));
        await Sender.Send(new CreateReviewCommand(gameId, Faker.Random.Guid(), 2, "Bad"));
        await Sender.Send(new CreateReviewCommand(gameId, Faker.Random.Guid(), 3, "Okay"));

        // Poll تا همه ۳ review پروسس بشن
        Result<GameResponse> gameResult = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<GameResponse?> g = await Sender.Send(new GetGameQuery(gameId));

            if (g.IsFailure || g.Value?.TotalReviews < 3)
                return Result.Failure<GameResponse>(
                    Error.Failure("Rating.NotUpdated", "Rating not updated yet"));

            return Result.Success(g.Value);
        });

        // Assert
        gameResult.Value.AverageRating.Should().Be(3.0m);
        gameResult.Value.TotalReviews.Should().Be(3);
    }
}