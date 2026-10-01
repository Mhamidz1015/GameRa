using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;

namespace GameRa.IntegrationTests.RefundCrossActs;

public sealed class RefundCrossActTests : BaseIntegrationTest
{
    public RefundCrossActTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task RefundOrder_ShouldRemoveGameFromLibrary()
    {
        // Arrange — full order flow
        Guid userId = await Sender.RegisterUserAsync();
        await Sender.CreateCustomerAsync(userId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(userId, gameId);
        Guid orderId = await Sender.CreateOrderAsync(userId);

        // Wait for Library to be populated
        await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<IReadOnlyCollection<LibraryItemResponse>> lib =
                await Sender.Send(new GetUserLibraryQuery(userId));

            if (lib.IsFailure || !lib.Value.Any(x => x.GameId == gameId))
                return Result.Failure<bool>(Error.Failure("Library.Empty", "Not yet"));

            return Result.Success(true);
        });

        // Act — refund
        await Sender.Send(new RefundOrderCommand(orderId, userId));

        // Poll تا game از library حذف بشه
        Result<bool> refundResult = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<IReadOnlyCollection<LibraryItemResponse>> lib =
                await Sender.Send(new GetUserLibraryQuery(userId));

            if (lib.Value.Any(x => x.GameId == gameId))
                return Result.Failure<bool>(
                    Error.Failure("Library.StillHasGame", "Game still in library"));

            return Result.Success(true);
        });

        // Assert
        refundResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RefundOrder_ShouldRemoveVerifiedPurchase()
    {
        // Arrange
        Guid userId = await Sender.RegisterUserAsync();
        await Sender.CreateCustomerAsync(userId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        await Sender.AddItemToCartAsync(userId, gameId);
        Guid orderId = await Sender.CreateOrderAsync(userId);

        // Wait for Library
        await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<IReadOnlyCollection<LibraryItemResponse>> lib =
                await Sender.Send(new GetUserLibraryQuery(userId));

            if (!lib.Value.Any(x => x.GameId == gameId))
                return Result.Failure<bool>(Error.Failure("x", "x"));

            return Result.Success(true);
        });

        // Create review (should be VerifiedPurchase = true)
        Result<Guid> reviewResult = await Sender.Send(
            new CreateReviewCommand(gameId, userId, 5, "Great game"));

        // Act — refund
        await Sender.Send(new RefundOrderCommand(orderId, userId));

        // Poll تا IsVerifiedPurchase false بشه
        // review باقی میمونه ولی VerifiedPurchase record حذف میشه
        Result<ReviewResponse> review = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<ReviewResponse> r = await Sender.Send(new GetReviewQuery(reviewResult.Value));

            if (r.IsFailure || r.Value.IsVerifiedPurchase)
                return Result.Failure<ReviewResponse>(
                    Error.Failure("Review.StillVerified", "VerifiedPurchase not removed yet"));

            return r;
        });

        // Assert
        review.IsSuccess.Should().BeTrue();
        review.Value.IsVerifiedPurchase.Should().BeFalse();
    }
}