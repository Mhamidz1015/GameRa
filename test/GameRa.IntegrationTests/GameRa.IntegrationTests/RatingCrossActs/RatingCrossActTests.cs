using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Games.Application.Games.GetGame;
using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using GameRa.Modules.Reviews.Application.Reviews.DeleteReview;

namespace GameRa.IntegrationTests.RatingCrossActs;

public sealed class RatingCrossActTests : BaseIntegrationTest
{
    public RatingCrossActTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task ReviewCreated_ShouldUpdateGameAverageRating()
    {
        // Arrange
        Guid gameId = await Sender.CreateGameInGamesModuleAsync();

        // Act
        Result<Guid> review = await Sender.Send(
            new CreateReviewCommand(gameId, Faker.Random.Guid(), 4, "Good game"));

        review.IsSuccess.Should().BeTrue();

        Result<GameResponse> game = await WaitForGameAsync(gameId, g => g.TotalReviews == 1);

        // Assert
        game.IsSuccess.Should().BeTrue();
        game.Value.TotalReviews.Should().Be(1);
        game.Value.AverageRating.Should().BeApproximately(4, 0.001);
    }

    [Fact]
    public async Task ReviewDeleted_ShouldUpdateGameAverageRating()
    {
        // Arrange
        Guid gameId = await Sender.CreateGameInGamesModuleAsync();
        Guid userId = Faker.Random.Guid();

        Result<Guid> review = await Sender.Send(
            new CreateReviewCommand(gameId, userId, 5, "Amazing"));

        review.IsSuccess.Should().BeTrue();

        Result<GameResponse> afterCreate = await WaitForGameAsync(gameId, g => g.TotalReviews == 1);
        afterCreate.IsSuccess.Should().BeTrue();

        // Act
        Result deleteResult = await Sender.Send(new DeleteReviewCommand(review.Value, userId));
        deleteResult.IsSuccess.Should().BeTrue();

        Result<GameResponse> afterDelete = await WaitForGameAsync(gameId, g => g.TotalReviews == 0);

        // Assert
        afterDelete.IsSuccess.Should().BeTrue();
        afterDelete.Value.TotalReviews.Should().Be(0);
        afterDelete.Value.AverageRating.Should().Be(0);
    }

    [Fact]
    public async Task MultipleReviews_ShouldCalculateCorrectAverage()
    {
        // Arrange
        Guid gameId = await Sender.CreateGameInGamesModuleAsync();

        // Act
        foreach ((int rating, string comment) in new[] { (4, "Good"), (2, "Bad"), (3, "Okay") })
        {
            Result<Guid> review = await Sender.Send(
                new CreateReviewCommand(gameId, Faker.Random.Guid(), rating, comment));

            review.IsSuccess.Should().BeTrue();
        }

        Result<GameResponse> game = await WaitForGameAsync(gameId, g => g.TotalReviews == 3);

        // Assert
        game.IsSuccess.Should().BeTrue();
        game.Value.TotalReviews.Should().Be(3);
        game.Value.AverageRating.Should().BeApproximately(3.0, 0.001);
    }

    private Task<Result<GameResponse>> WaitForGameAsync(Guid gameId, Func<GameResponse, bool> condition)
    {
        return Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<GameResponse?> result = await Sender.Send(new GetGameQuery(gameId));

            if (result.IsFailure || result.Value is null || !condition(result.Value))
            {
                return Result.Failure<GameResponse>(
                    Error.Failure("Rating.NotUpdated", "Rating not updated yet"));
            }

            return Result.Success(result.Value);
        });
    }
}
