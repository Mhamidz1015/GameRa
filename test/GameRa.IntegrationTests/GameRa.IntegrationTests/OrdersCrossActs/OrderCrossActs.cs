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
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.IntegrationTests.OrdersCrossActs
{
    public sealed class OrderCrossActs : BaseIntegrationTest
    {
        public OrderCrossActs(IntegrationTestWebAppFactory factory) :
        base(factory)
        {
        }

        [Fact]
        public async Task Order_Should_CreateVerifiedPurchase_InReviewsModule()
        {
            // Arrange — Register user
            Guid userId = await Sender.RegisterUserAsync();

            // Create customer in Store
            await Sender.CreateCustomerAsync(userId);

            // Add game
            Guid gameId = Faker.Random.Guid();
            await Sender.AddGameAsync(gameId);

            // Add to cart and order
            await Sender.AddItemToCartAsync(userId, gameId);
            await Sender.CreateOrderAsync(userId);

            // Wait for Library to be updated (OrderCompleted → Library)
            await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
            {
                Result<IReadOnlyCollection<LibraryItemResponse>> lib =
                    await Sender.Send(new GetUserLibraryQuery(userId));

                if (lib.IsFailure || !lib.Value.Any(x => x.GameId == gameId))
                    return Result.Failure<IReadOnlyCollection<LibraryItemResponse>>(
                        Error.Failure("Library.Empty", "Game not in library yet"));

                return lib;
            });

            // Act — Now write a review (should be VerifiedPurchase = true via VerifiedPurchaseRepository)
            // Wait for VerifiedPurchase to be created (OrderCompleted → Reviews inbox)
            Result<Guid> reviewResult = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
            {
                Result<Guid> r = await Sender.Send(new CreateReviewCommand(
                    gameId, userId, Faker.Random.Int(4, 5),
                    Faker.Lorem.Sentence(),
                    verifiedPurchase: true));

                if (r.IsFailure)
                    return Result.Failure<Guid>(r.Error);

                return r;
            });

            // Assert
            reviewResult.IsSuccess.Should().BeTrue();

            Result<ReviewResponse> getReview = await Sender.Send(
                new GetReviewQuery(reviewResult.Value));

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
                gameId, userId, Faker.Random.Int(1, 3),
                Faker.Lorem.Sentence(),
                verifiedPurchase: false));

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
            Result<Guid> userResult = await Sender.Send(new RegisterUserCommand(
                Faker.Internet.Email(),
                Faker.Internet.Password(12),
                Faker.Internet.UserName(),
                DateTime.UtcNow));

            Guid userId = userResult.Value;

            await Sender.CreateCustomerAsync(userId);

            Guid gameId = Faker.Random.Guid();
            await Sender.AddGameAsync(gameId);

            await Sender.AddItemToCartAsync(userId, gameId);
            await Sender.CreateOrderAsync(userId);

            // Wait for Library
            await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
            {
                Result<bool> ownership = await Sender.Send(
                    new CheckGameOwnershipQuery(userId, gameId));

                if (ownership.IsFailure || !ownership.Value)
                    return Result.Failure<bool>(
                        Error.Failure("Ownership.NotSet", "Ownership not set yet"));

                return ownership;
            });

            // Assert ownership
            Result<bool> finalOwnership = await Sender.Send(
                new CheckGameOwnershipQuery(userId, gameId));

            finalOwnership.IsSuccess.Should().BeTrue();
            finalOwnership.Value.Should().BeTrue();
        }

        // ─────────────────────────────────────────────
        // Full flow: Register → Order → Review
        // ─────────────────────────────────────────────

        [Fact]
        public async Task FullFlow_RegisterOrder_ThenReview_ShouldSucceed()
        {
            // Step 1: Register user
            Guid userId = await Sender.RegisterUserAsync();

            // Step 2: Create customer in Store
            await Sender.CreateCustomerAsync(userId);

            // Step 3: Add game to Store
            Guid gameId = Faker.Random.Guid();
            await Sender.AddGameAsync(gameId);

            // Step 4: Apply a discount to this game
            await Sender.CreateGameDiscountAsync(gameId, DiscountType.Percentage, 10m);

            // Step 5: Verify discount appears for this game
            Result<IReadOnlyCollection<DiscountResponse>> discountResult = await Sender.Send(
                new GetActiveDiscountsForGameQuery(gameId, Faker.Random.Guid()));

            discountResult.Value.Should().Contain(d => d.GameId == gameId);

            // Step 6: Add to cart and order
            await Sender.AddItemToCartAsync(userId, gameId);
            await Sender.CreateOrderAsync(userId);

            // Step 7: Write a review for the game
            Result<Guid> reviewResult = await Sender.Send(new CreateReviewCommand(
                gameId, userId, Faker.Random.Int(3, 5),
                Faker.Lorem.Sentence(),
                verifiedPurchase: false));

            reviewResult.IsSuccess.Should().BeTrue();

            // Step 8: Verify average rating updated
            Result<AverageRatingResponse> ratingResult = await Sender.Send(
                new GetAverageRatingByGameIdQuery(gameId));

            ratingResult.IsSuccess.Should().BeTrue();
            ratingResult.Value.TotalReviews.Should().BeGreaterThanOrEqualTo(1);

        }

    }
}

