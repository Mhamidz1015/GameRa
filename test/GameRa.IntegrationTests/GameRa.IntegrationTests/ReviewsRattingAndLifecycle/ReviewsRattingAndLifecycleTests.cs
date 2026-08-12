using Bogus;
using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Reviews.Application.Reviews.GetAverageRatingByGameId;
using GameRa.Modules.Reviews.Application.Reviews.GetReviewsByGameId;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.IntegrationTests.ReviewsRattingAndLifecycle
{
    public sealed class ReviewsRattingAndLifecycleTests : BaseIntegrationTest
    {
        public ReviewsRattingAndLifecycleTests(IntegrationTestWebAppFactory factory) :
         base(factory)
        {
        }

        // ─────────────────────────────────────────────
        // Reviews → Games (cross-module average rating)
        // ─────────────────────────────────────────────

        [Fact]
        public async Task MultipleReviews_ShouldAffect_AverageRating_Correctly()
        {
            // Arrange
            Guid gameId = Faker.Random.Guid();

            // Act — multiple users review same game
            await Sender.CreateReviewAsync(gameId, Faker.Random.Guid(), 5);
            await Sender.CreateReviewAsync(gameId, Faker.Random.Guid(), 1);
            await Sender.CreateReviewAsync(gameId, Faker.Random.Guid(), 4);
            await Sender.CreateReviewAsync(gameId, Faker.Random.Guid(), 4);

            Result<AverageRatingResponse> result = await Sender.Send(
                new GetAverageRatingByGameIdQuery(gameId));

            // Assert — (5+1+4+4)/4 = 3.5
            result.IsSuccess.Should().BeTrue();
            result.Value.AverageRating.Should().Be(3.5m);
            result.Value.TotalReviews.Should().Be(4);
        }

        [Fact]
        public async Task Reviews_ShouldBe_Isolated_BetweenGames()
        {
            // Arrange
            Guid gameId1 = Faker.Random.Guid();
            Guid gameId2 = Faker.Random.Guid();

            // Act — reviews for different games
            await Sender.CreateReviewAsync(gameId1, Faker.Random.Guid(), 5);
            await Sender.CreateReviewAsync(gameId1, Faker.Random.Guid(), 5);
            await Sender.CreateReviewAsync(gameId2, Faker.Random.Guid(), 1);

            Result<AverageRatingResponse> game1Rating = await Sender.Send(
                new GetAverageRatingByGameIdQuery(gameId1));

            Result<AverageRatingResponse> game2Rating = await Sender.Send(
                new GetAverageRatingByGameIdQuery(gameId2));

            // Assert — ratings are isolated per game
            game1Rating.Value.AverageRating.Should().Be(5);
            game1Rating.Value.TotalReviews.Should().Be(2);

            game2Rating.Value.AverageRating.Should().Be(1);
            game2Rating.Value.TotalReviews.Should().Be(1);
        }

        // ─────────────────────────────────────────────
        // Reviews listing cross-check
        // ─────────────────────────────────────────────

        [Fact]
        public async Task GetReviewsByGameId_ShouldReturn_CorrectReviews_AfterMultipleOperations()
        {
            Guid gameId = Faker.Random.Guid();
            Guid userId1 = Faker.Random.Guid();
            Guid userId2 = Faker.Random.Guid();
            Guid userId3 = Faker.Random.Guid();

            // Create 3 reviews
            await Sender.CreateReviewAsync(gameId, userId1, 5);
            await Sender.CreateReviewAsync(gameId, userId2, 3);
            Guid review3 = await Sender.CreateReviewAndGetIdAsync(gameId, userId3, 1);

            // Get all reviews
            Result<IReadOnlyCollection<Modules.Reviews.Application.Reviews.GetReview.ReviewResponse>> allReviews =
                await Sender.Send(new GetReviewsByGameIdQuery(gameId));

            allReviews.Value.Should().HaveCount(3);

            // Average should be (5+3+1)/3 = 3
            Result<AverageRatingResponse> avgBefore = await Sender.Send(
                new GetAverageRatingByGameIdQuery(gameId));
            avgBefore.Value.AverageRating.Should().Be(3.0m);

            // Delete one review
            await Sender.DeleteReviewAsync(review3, userId3);

            // Reviews should now be 2
            Result<IReadOnlyCollection<Modules.Reviews.Application.Reviews.GetReview.ReviewResponse>> afterDelete =
                await Sender.Send(new GetReviewsByGameIdQuery(gameId));

            afterDelete.Value.Should().HaveCount(2);
            afterDelete.Value.Should().NotContain(r => r.ReviewId == review3);
        }

    }
}