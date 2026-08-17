using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Discounts.Application.Discounts.GetActiveDiscountsForGame;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Domain;
using GameRa.Modules.Library.Application.LibraryItems.CheckGameOwnership;
using GameRa.Modules.Library.Application.LibraryItems.GetUserLibrary;
using GameRa.Modules.Reviews.Application.Reviews.CreateReview;
using GameRa.Modules.Reviews.Application.Reviews.GetAverageRatingByGameId;
using GameRa.Modules.Reviews.Application.Reviews.GetReview;
using GameRa.Modules.Users.Application.Users.RegisterUser;

namespace GameRa.IntegrationTests.OrdersCrossActs;

public sealed class OrderCrossActs : BaseIntegrationTest
{
    public OrderCrossActs(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Order_Should_CreateVerifiedPurchase_InReviewsModule()
    {
        // Arrange
        Guid userId = await Sender.RegisterUserAsync();
        await Sender.CreateCustomerAsync(userId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);

        await Sender.AddItemToCartAsync(userId, gameId);
        await Sender.CreateOrderAsync(userId);

        // Wait for Library to be updated → proves OrderCompleted was published
        await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<IReadOnlyCollection<LibraryItemResponse>> lib =
                await Sender.Send(new GetUserLibraryQuery(userId));

            if (lib.IsFailure || !lib.Value.Any(x => x.GameId == gameId))
                return Result.Failure<IReadOnlyCollection<LibraryItemResponse>>(
                    Error.Failure("Library.Empty", "Game not in library yet"));

            return lib;
        });

        // Wait for Reviews inbox to process OrderCompleted → VerifiedPurchase created
        // We poll GetReview after creating it to check IsVerifiedPurchase
        // Strategy: create review once, then poll until IsVerifiedPurchase = true
        Result<Guid> reviewResult = await Sender.Send(new CreateReviewCommand(
            gameId, userId,
            Faker.Random.Int(4, 5),
            Faker.Lorem.Sentence()));

        reviewResult.IsSuccess.Should().BeTrue();

        // Poll until Reviews inbox processes the event and updates IsVerifiedPurchase
        Result<ReviewResponse> getReview = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<ReviewResponse> r = await Sender.Send(new GetReviewQuery(reviewResult.Value));

            if (r.IsFailure || !r.Value.IsVerifiedPurchase)
                return Result.Failure<ReviewResponse>(
                    Error.Failure("Review.NotVerified", "VerifiedPurchase not set yet"));

            return r;
        });

        // Assert
        getReview.IsSuccess.Should().BeTrue();
        getReview.Value.IsVerifiedPurchase.Should().BeTrue();
        getReview.Value.UserId.Should().Be(userId);
        getReview.Value.GameId.Should().Be(gameId);
    }

    [Fact]
    public async Task NonPurchasedGame_Review_ShouldNotBe_VerifiedPurchase()
    {
        // Arrange — user who never bought the game
        Guid userId = Faker.Random.Guid();
        Guid gameId = Faker.Random.Guid();

        // Act — write review without purchasing
        Result<Guid> reviewResult = await Sender.Send(new CreateReviewCommand(
            gameId, userId,
            Faker.Random.Int(1, 3),
            Faker.Lorem.Sentence()));

        // Assert
        reviewResult.IsSuccess.Should().BeTrue();

        Result<ReviewResponse> getReview = await Sender.Send(
            new GetReviewQuery(reviewResult.Value));

        getReview.Value.IsVerifiedPurchase.Should().BeFalse();
    }

    [Fact]
    public async Task Order_Should_AddGameToLibrary_AndAllowOwnershipCheck()
    {
        // Arrange
        Guid userId = await Sender.RegisterUserAsync();
        await Sender.CreateCustomerAsync(userId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);

        await Sender.AddItemToCartAsync(userId, gameId);
        await Sender.CreateOrderAsync(userId);

        // Poll until ownership is confirmed
        await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            Result<bool> ownership = await Sender.Send(
                new CheckGameOwnershipQuery(userId, gameId));

            if (ownership.IsFailure || !ownership.Value)
                return Result.Failure<bool>(
                    Error.Failure("Ownership.NotSet", "Ownership not set yet"));

            return ownership;
        });

        // Assert
        Result<bool> finalOwnership = await Sender.Send(
            new CheckGameOwnershipQuery(userId, gameId));

        finalOwnership.IsSuccess.Should().BeTrue();
        finalOwnership.Value.Should().BeTrue();
    }

    [Fact]
    public async Task FullFlow_RegisterOrder_ThenReview_ShouldSucceed()
    {
        // Step 1 & 2: Register user + Create customer
        Guid userId = await Sender.RegisterUserAsync();
        await Sender.CreateCustomerAsync(userId);

        // Step 3: Add game
        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);

        // Step 4: Apply discount
        await Sender.CreateGameDiscountAsync(gameId, DiscountType.Percentage, 10m);

        // Step 5: Verify discount appears
        Result<IReadOnlyCollection<DiscountResponse>> discountResult = await Sender.Send(
            new GetActiveDiscountsForGameQuery(gameId, Faker.Random.Guid()));

        discountResult.Value.Should().Contain(d => d.GameId == gameId);

        // Step 6: Add to cart and order
        await Sender.AddItemToCartAsync(userId, gameId);
        await Sender.CreateOrderAsync(userId);

        // Step 7: Write review
        Result<Guid> reviewResult = await Sender.Send(new CreateReviewCommand(
            gameId, userId,
            Faker.Random.Int(3, 5),
            Faker.Lorem.Sentence()));

        reviewResult.IsSuccess.Should().BeTrue();

        // Step 8: Verify average rating updated
        Result<AverageRatingResponse> ratingResult = await Sender.Send(
            new GetAverageRatingByGameIdQuery(gameId));

        ratingResult.IsSuccess.Should().BeTrue();
        ratingResult.Value.TotalReviews.Should().BeGreaterThanOrEqualTo(1);
    }
}