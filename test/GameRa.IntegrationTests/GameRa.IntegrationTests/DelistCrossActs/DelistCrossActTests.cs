using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Games.DelistGame;

namespace GameRa.IntegrationTests.DelistCrossActs;

public sealed class DelistCrossActTests : BaseIntegrationTest
{
    public DelistCrossActTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task DelistGame_ShouldMarkGameAsDelistedInStore()
    {
        // Arrange
        Guid customerId = Faker.Random.Guid();
        await Sender.CreateCustomerAsync(customerId);

        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);

        await Sender.AddItemToCartAsync(customerId, gameId);

        // Act — delist the game
        Result delistResult = await Sender.Send(new DelistGameCommand(gameId));

        delistResult.IsSuccess.Should().BeTrue();

        // Poll تا Store inbox پروسس کنه
        Result<bool> result = await Poller.WaitAsync(TimeSpan.FromSeconds(30), async () =>
        {
            // AddToCart باید fail بشه چون game delisted شده
            Result addResult = await Sender.Send(
                new AddItemToCartCommand(customerId, gameId));

            if (addResult.IsSuccess)
                return Result.Failure<bool>(
                    Error.Failure("Game.NotDelisted", "Game not yet marked as delisted"));

            return Result.Success(true);
        });

        result.IsSuccess.Should().BeTrue();
    }
}