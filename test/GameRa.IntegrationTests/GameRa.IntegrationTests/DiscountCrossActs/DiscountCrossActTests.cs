using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Discounts.Application.Discounts.GetActiveDiscountsForGame;
using GameRa.Modules.Discounts.Application.Discounts.GetDiscount;
using GameRa.Modules.Discounts.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameRa.IntegrationTests.DiscountCrossActs
{
    public sealed class DiscountCrossActTests : BaseIntegrationTest
    {
        public DiscountCrossActTests(IntegrationTestWebAppFactory factory) :
        base(factory)
        {
        }

        // ─────────────────────────────────────────────
        // Discounts → Games (cross-module)
        // ─────────────────────────────────────────────

        [Fact]
        public async Task GameDiscount_ShouldAppear_InActiveDiscountsForGame()
        {
            // Arrange
            Guid gameId = Faker.Random.Guid();
            Guid categoryId = Faker.Random.Guid();
            // Act
            await Sender.CreateGameDiscountAsync(gameId, DiscountType.Percentage, 25m);

            Result<IReadOnlyCollection<DiscountResponse>> result = await Sender.Send(
                new GetActiveDiscountsForGameQuery(gameId, categoryId));

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Contain(d =>
                d.GameId == gameId &&
                d.Amount == 25m &&
                d.Scope == (int)DiscountScope.Game);
        }

        [Fact]
        public async Task GlobalDiscount_ShouldAppear_ForAllGames()
        {
            // Arrange
            Guid gameId1 = Faker.Random.Guid();
            Guid gameId2 = Faker.Random.Guid();
            Guid categoryId = Faker.Random.Guid();
            string code = await Sender.CreateGlobalDiscountAsync(DiscountType.Percentage, 10m);

            // Act — create one global discount

            // Query for two completely different games
            Result<IReadOnlyCollection<DiscountResponse>> result1 = await Sender.Send(
                new GetActiveDiscountsForGameQuery(gameId1, categoryId));

            Result<IReadOnlyCollection<DiscountResponse>> result2 = await Sender.Send(
                new GetActiveDiscountsForGameQuery(gameId2, categoryId));

            // Assert — both games see the global discount
            result1.Value.Should().Contain(d => d.Code == code && d.Scope == (int)DiscountScope.Global);
            result2.Value.Should().Contain(d => d.Code == code && d.Scope == (int)DiscountScope.Global);
        }

        [Fact]
        public async Task MultipleDiscountTypes_ShouldAllAppear_ForSameGame()
        {
            // Arrange
            Guid gameId = Faker.Random.Guid();
            Guid categoryId = Faker.Random.Guid();

            // Game-specific discount
            await Sender.CreateGameDiscountAsync(gameId, DiscountType.Percentage, 15m);

            // Global discount
            await Sender.CreateGlobalDiscountAsync(DiscountType.FixedAmount, 5m);

            // Act
            Result<IReadOnlyCollection<DiscountResponse>> result = await Sender.Send(
                new GetActiveDiscountsForGameQuery(gameId, categoryId));

            // Assert — both discounts visible for this game
            result.Value.Should().Contain(d => d.GameId == gameId && d.Scope == (int)DiscountScope.Game);
            result.Value.Should().Contain(d => d.Scope == (int)DiscountScope.Global);
        }
    }
}
